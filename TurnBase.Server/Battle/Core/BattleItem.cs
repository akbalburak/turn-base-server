using TurnBase.Server.Battle.Core.Skills;
using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle.Core
{
    public class BattleItem : IDisposable
    {
        private bool _gameStarted;

        private BattleLevelData _levelData;

        private BattleDifficulityData _difficulityData;
        private BattleLevels _difficulity;

        private BattleTurnHandler _turnHandler;

        private BattleUser[] _users;

        private BattleWave[] _waves;
        private BattleWave _currentWave;

        private int _skillIdCounter;
        private int _unitIdCounter;
        private int _dataIdCounter;

        private bool _disposed;

        public BattleItem(BattleUser[] users,
            BattleLevelData levelData,
            BattleLevels difficulity)
        {
            _users = users;

            _levelData = levelData;
            _difficulityData = levelData.GetDifficulityData(difficulity);

            _waves = _difficulityData.Waves.ToArray();
            _currentWave = _waves[0];

            // WE CREATE IDS FOR UNITS.
            foreach (BattleWave wave in _waves)
            {
                foreach (BattleNpcUnit unit in wave.Units)
                {
                    unit.SetId(++_unitIdCounter);
                    unit.SetTeam(2);
                }
            }

            // WE CREATE IDS FOR USERS.
            foreach (BattleUser user in _users)
            {
                user.SetId(++_unitIdCounter);
                user.AddSkill(new BattleDoubleSlashSkill(++_skillIdCounter, this, user));
                user.SetTeam(1);
            }

            _turnHandler = new BattleTurnHandler(this, users, _currentWave.Units);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
        }

        public void ExecuteAction(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            switch (requestData.BattleAction)
            {
                case BattleActions.LoadAll:
                    SendAllReqDataToClient(socketUser, requestData);
                    break;
                case BattleActions.IamReady:
                    StartGame();
                    break;
                case BattleActions.TurnUpdated:
                    break;
                case BattleActions.UnitBasicAttack:
                    PlayerBasicAttack(socketUser, requestData);
                    break;
                case BattleActions.UnitUseSkill:
                    PlayerUseSkill(socketUser, requestData);
                    break;
            }
        }
        private void StartGame()
        {
            _gameStarted = true;

            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
        private void PlayerUseSkill(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleSkillUseDTO useData = requestData.GetRequestData<BattleSkillUseDTO>();

            // WE GET THE PLAYER.
            BattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            currentUser.UseSkill(useData);
        }
        private void PlayerBasicAttack(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // WE GET THE PLAYER.
            BattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            // WE GET A RANDOM ENEMY.
            BattleAttackUseDTO attackUseData = requestData.GetRequestData<BattleAttackUseDTO>();
            BattleUnitAttack targetEnemy = (BattleUnitAttack)GetUnit(attackUseData.TargetUniqueId);
            if (targetEnemy == null)
                return;

            // ATTACK TO PLAYER.
            int damage = currentUser.GetDamage(targetEnemy);
            targetEnemy.Attack(currentUser, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(currentUser.Id,
                    targetEnemy.Id,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            EndTurn();
        }
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            BattleUnitAttack currentTurnUnit = _turnHandler.GetCurrentTurnUnit();
            while (currentTurnUnit is not BattleUser)
            {
                // WE GET A RANDOM ENEMY.
                BattleUser firstUserToAttack = _users.FirstOrDefault(x => !x.IsDeath);
                if (firstUserToAttack == null)
                    break;

                // ATTACK TO PLAYER.
                int damage = currentTurnUnit.GetDamage(firstUserToAttack);
                firstUserToAttack.Attack(currentTurnUnit, damage);

                // WE WILL SEND THE DAMAGE DATA.
                BattleAttackDTO attackData = new BattleAttackDTO();
                attackData.AddAttack(
                    new BattleAttackItemDTO(currentTurnUnit.Id,
                        firstUserToAttack.Id,
                        damage
                    )
                );
                SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

                // FINALIZE UNIT TURN AND SKIP TO NEXT UNIT.
                _turnHandler.SkipToNextTurn();
                currentTurnUnit = _turnHandler.GetCurrentTurnUnit();
            }
        }

        private void SendAllReqDataToClient(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Waves = _waves.Select(y => new BattleWaveDTO
                {
                    Units = y.Units.Select(z => new BattleNpcUnitDTO
                    {
                        AttackSpeed = z.AttackSpeed,
                        Health = z.Health,
                        UniqueId = z.Id,
                        MaxDamage = z.MaxDamage,
                        MinDamage = z.MinDamage,
                        Position = z.Position,
                        MaxHealth = z.MaxHealth,
                        IsDead = z.IsDeath,
                        UnitId = z.UnitId,
                        TeamIndex = z.TeamIndex
                    }).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    UniqueId = z.Id,
                    AttackSpeed = z.AttackSpeed,
                    Health = z.Health,
                    MaxDamage = z.MaxDamage,
                    MinDamage = z.MinDamage,
                    Position = z.Position,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.MaxHealth,
                    IsDead = z.IsDeath,
                    TeamIndex = z.TeamIndex,
                    Skills = z.Skills.Select(v => new BattleSkillDTO
                    {
                        LeftTurnToUse = v.LeftTurnToUse,
                        UniqueID = v.UniqueId,
                        Skill = v.Skill,
                        TurnCooldown = v.TurnCooldown,
                    }).ToArray()
                }).ToArray()
            };

            SendToUser(socketUser, BattleActions.LoadAll, loadData);
        }

        public void SendToAllUsers(BattleActions battleAction, object data)
        {
            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                ++_dataIdCounter,
                battleAction,
                data
            );

            foreach (BattleUser user in _users)
            {
                if (!user.IsConnected)
                    continue;

                user.SocketUser.AddToUnExpectedAfterSendIt(dataToSend);
            }
        }
        public void SendToUser(SocketUser user, BattleActions battleAction, object data)
        {
            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                ++_dataIdCounter,
                battleAction,
                data
            );

            user.AddToUnExpectedAfterSendIt(dataToSend);
        }

        public BattleUser GetUser(SocketUser socketUser)
        {
            return _users.FirstOrDefault(y => y.SocketUser == socketUser);
        }
        public BattleUnit GetUnit(int targetUnitID)
        {
            BattleUser? user = _users.FirstOrDefault(y => y.Id == targetUnitID);
            if (user != null)
                return user;

            BattleNpcUnit unit = _currentWave.Units.FirstOrDefault(y => y.Id == targetUnitID);
            if (unit != null)
                return unit;

            return null;
        }
        public BattleUnit GetAliveEnemyUnit(BattleUnit owner)
        {
            return _currentWave.Units.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.Id != owner.Id);
        }

        public void EndTurn()
        {
            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
    }
}
