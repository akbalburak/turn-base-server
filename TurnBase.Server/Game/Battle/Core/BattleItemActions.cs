using TurnBase.Server.Extends;
using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Pathfinding;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void ExecuteAction(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // IF THE GAME IS OVER NO LONGER ACTIONS CAN BE EXECUTED.
            if (_gameOver)
                return;

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
                case BattleActions.MovePlayerUnit:
                    PlayerUnitMove(socketUser, requestData);
                    break;
            }
        }

        private void PlayerUnitMove(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleUnitMoveRequestDTO moveData = requestData.GetRequestData<BattleUnitMoveRequestDTO>();

            // WE GET THE PLAYER.
            IBattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            // WE MAKE SURE MOVE INDEX IS VALID.
            if (moveData.ToIndex < 0 || moveData.ToIndex >= _nodes.Length)
                return;

            // WE GET THE POINTS.
            IAstarNode fromPoint = currentUser.Node;
            IAstarNode targetPoint = _nodes[moveData.ToIndex];

            // WE LOOK FOR THE PATH.
            IAstarNode[] path = AStar.FindPath(_nodes, fromPoint, targetPoint);
            if (path.Length == 0)
                return;

            currentUser.SetPosition(targetPoint);

            SendToAllUsers(BattleActions.MovePlayerUnit, new BattleUnitMoveResponseDTO
            {
                UnitUniqueId = currentUser.UniqueId,
                ToIndex = moveData.ToIndex,
            });
        }

        private void StartGame()
        {
            _gameStarted = true;

            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }
        private void PlayerUseSkill(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleSkillUseDTO useData = requestData.GetRequestData<BattleSkillUseDTO>();

            // WE GET THE PLAYER.
            IBattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            currentUser.UseSkill(useData);
        }
        private void PlayerBasicAttack(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // WE GET THE PLAYER.
            IBattleUser currentUser = GetUser(socketUser);
            if (currentUser == null)
                return;

            // MAKE SURE PLAYER TURN.
            if (!_turnHandler.IsUnitTurn(currentUser))
                return;

            // WE GET A RANDOM ENEMY.
            BattleAttackUseDTO attackUseData = requestData.GetRequestData<BattleAttackUseDTO>();
            IBattleUnit targetEnemy = GetUnit(attackUseData.TargetUniqueId);
            if (targetEnemy == null)
                return;

            // ATTACK TO PLAYER.
            int damage = currentUser.GetBaseDamage(targetEnemy);
            currentUser.AttackToUnit(targetEnemy, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(currentUser.UniqueId,
                    targetEnemy.UniqueId,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            FinalizeTurn();
        }
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            IBattleUnit attacker = _turnHandler.GetCurrentTurnUnit();
            if (attacker is IBattleUser)
                return;

            // WE GET A RANDOM ENEMY.
            IBattleUnit defender = _allUnits.Find(x => x.TeamIndex != attacker.TeamIndex && !x.IsDeath);
            if (defender == null)
                return;

            // ATTACK TO PLAYER.
            int damage = attacker.GetBaseDamage(defender);
            attacker.AttackToUnit(defender, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(attacker.UniqueId,
                    defender.UniqueId,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            // FINALIZE UNIT TURN AND SKIP TO NEXT UNIT.
            FinalizeTurn();
        }
        private void SendAllReqDataToClient(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            IBattleUser user = _users.FirstOrDefault(y => y.SocketUser == socketUser);

            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Difficulity = _difficulity,
                Waves = _waves.Select(y => new BattleWaveDTO
                {
                    Units = y.Units.Select(z => new BattleNpcUnitDTO
                    {
                        AttackSpeed = z.Stats.AttackSpeed,
                        Health = z.Health,
                        Mana = z.Mana,
                        UniqueId = z.UniqueId,
                        Damage = z.Stats.Damage,
                        Position = z.Position,
                        MaxHealth = z.Stats.MaxHealth,
                        MaxMana = z.Stats.MaxMana,
                        IsDead = z.IsDeath,
                        UnitId = z.UnitId,
                        TeamIndex = z.TeamIndex,
                        NodeIndex = _nodes.IndexOf(z.Node),
                    }).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    UniqueId = z.UniqueId,
                    AttackSpeed = z.Stats.AttackSpeed,
                    Health = z.Health,
                    Mana = z.Mana,
                    Damage = z.Stats.Damage,
                    Position = z.Position,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.Stats.MaxHealth,
                    MaxMana = z.Stats.MaxMana,
                    IsDead = z.IsDeath,
                    TeamIndex = z.TeamIndex,
                    Skills = z.Skills.Select(v => new BattleSkillDTO
                    {
                        LeftTurnToUse = v.LeftTurnToUse,
                        UniqueID = v.UniqueId,
                        Skill = v.SkillData.ItemSkill,
                        TurnCooldown = v.TurnCooldown,
                        UsageManaCost = v.UsageManaCost
                    }).ToArray(),
                    NodeIndex = _nodes.IndexOf(z.Node),
                }).ToArray()
            };

            SendToUser(user, BattleActions.LoadAll, loadData);
        }

    }
}
