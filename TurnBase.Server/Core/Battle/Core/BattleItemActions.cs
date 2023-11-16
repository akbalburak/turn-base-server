using TurnBase.DTOLayer.Enums;
using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Core
{
    public partial class BattleItem
    {
        public void ExecuteAction(SocketUser socketUser, BattleActionRequestDTO requestData)
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
            BattleUnit targetEnemy = GetUnit(attackUseData.TargetUniqueId);
            if (targetEnemy == null)
                return;

            // ATTACK TO PLAYER.
            int damage = currentUser.GetDamage(targetEnemy);
            currentUser.AttackTo(targetEnemy, damage);

            // WE WILL SEND THE DAMAGE DATA.
            BattleAttackDTO attackData = new BattleAttackDTO();
            attackData.AddAttack(
                new BattleAttackItemDTO(currentUser.UniqueId,
                    targetEnemy.UniqueId,
                    damage
                )
            );
            SendToAllUsers(BattleActions.UnitBasicAttack, attackData);

            EndTurn();
        }
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            BattleUnit attacker = _turnHandler.GetCurrentTurnUnit();
            if (attacker is BattleUser)
                return;

            // WE GET A RANDOM ENEMY.
            BattleUnit defender = _allUnits.Find(x => x.TeamIndex != attacker.TeamIndex && !x.IsDeath);
            if (defender == null)
                return;

            // ATTACK TO PLAYER.
            int damage = attacker.GetDamage(defender);
            attacker.AttackTo(defender, damage);

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
            EndTurn();
        }
        private void SendAllReqDataToClient(SocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleUser? user = _users.FirstOrDefault(y => y.SocketUser == socketUser);

            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Difficulity = _difficulity,
                Waves = _waves.Select(y => new BattleWaveDTO
                {
                    Units = y.Units.Select(z => new BattleNpcUnitDTO
                    {
                        AttackSpeed = z.Stats.AttackSpeed,
                        Health = z.Health,
                        UniqueId = z.UniqueId,
                        Damage = z.Stats.Damage,
                        Position = z.Position,
                        MaxHealth = z.Stats.MaxHealth,
                        IsDead = z.IsDeath,
                        UnitId = z.UnitId,
                        TeamIndex = z.TeamIndex
                    }).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    UniqueId = z.UniqueId,
                    AttackSpeed = z.Stats.AttackSpeed,
                    Health = z.Health,
                    Damage = z.Stats.Damage,
                    Position = z.Position,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.Stats.MaxHealth,
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

            SendToUser(user, BattleActions.LoadAll, loadData);
        }

    }
}
