using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.Enums;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle.Core
{
    public class BattleItem : IDisposable
    {
        private int _idCounter;
        private bool _gameStarted;

        private BattleLevelData _levelData;

        private BattleDifficulityData _difficulityData;
        private BattleLevels _difficulity;

        private BattleTurnHandler _turnHandler;

        private BattleUser[] _users;

        private BattleWave[] _waves;
        private BattleWave _currentWave;

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

            _turnHandler = new BattleTurnHandler(
                users,
                _currentWave.Units
            );
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
                case BattleActions.PlayerDoAction:
                    PlayerDoAnAction(socketUser);
                    break;
            }
        }
        private void StartGame()
        {
            _gameStarted = true;

            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
        private void PlayerDoAnAction(SocketUser user)
        {
            // DO ATTACK STUFFS OR OTHER SKILLS.
            var currentUnit = _turnHandler.GetCurrentTurnUnit();
            if (currentUnit is not BattleUser)
                return;

            BattleUser currentUser = currentUnit as BattleUser;
            if (user != currentUser?.SocketUser)
                return;

            // WE GET A RANDOM ENEMY.
            BattleNpcUnit firstEnemyToAttack = _currentWave.Units.FirstOrDefault(x=> !x.IsDeath);
            if (firstEnemyToAttack == null)
                return;

            // ATTACK TO PLAYER.
            int damage = currentUser.GetDamage(firstEnemyToAttack);
            firstEnemyToAttack.Attack(currentUser, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(currentUser.Id,
                    firstEnemyToAttack.Id,
                    damage
                )
            );
            SendToAllUsers(BattleActions.Attack, attackData);

            // FINALLY WE SKIP TO NEXT PLAYER.
            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            BattleUnitAttack currentTurnUnit = _turnHandler.GetCurrentTurnUnit();
            while (currentTurnUnit is not BattleUser)
            {
                // WE GET A RANDOM ENEMY.
                BattleUser firstUserToAttack = _users.FirstOrDefault(x=> !x.IsDeath);
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
                SendToAllUsers(BattleActions.Attack, attackData);

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
                        Id = z.Id,
                        MaxDamage = z.MaxDamage,
                        MinDamage = z.MinDamage,
                        Position = z.Position,
                        Unit = z.UnitType,
                        MaxHealth = z.MaxHealth,
                        IsDead = z.IsDeath
                    }).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    Id = z.Id,
                    AttackSpeed = z.AttackSpeed,
                    Health = z.Health,
                    MaxDamage = z.MaxDamage,
                    MinDamage = z.MinDamage,
                    Position = z.Position,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.MaxHealth,
                    IsDead = z.IsDeath
                }).ToArray()
            };

            SendToUser(socketUser, BattleActions.LoadAll, loadData);
        }

        private void SendToAllUsers(BattleActions battleAction, object data)
        {
            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                ++_idCounter,
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
        private void SendToUser(SocketUser user, BattleActions battleAction, object data)
        {
            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                ++_idCounter,
                battleAction,
                data
            );

            user.AddToUnExpectedAfterSendIt(dataToSend);
        }
    }
}
