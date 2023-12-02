using TurnBase.Server.Extends;
using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
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
                case BattleActions.FinalizeTurn:
                    FinalizePlayerTurn(socketUser);
                    break;
                case BattleActions.UnitUseSkill:
                    PlayerUseSkill(socketUser, requestData);
                    break;
            }
        }

        private void FinalizePlayerTurn(ISocketUser socketUser)
        {
            IBattleUser user = GetUser(socketUser);
            if (!_turnHandler.IsUnitTurn(user))
                return;

            _turnHandler.SkipToNextTurn();
            BattleTillAnyPlayerTurn();
        }

        private void StartGame()
        {
            _gameStarted = true;
            _turnHandler.SkipToNextTurn();
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
        private void BattleTillAnyPlayerTurn()
        {
            // WE LOOP TILL PLAYER TURN.
            IBattleUnit attacker = _turnHandler.GetCurrentTurnUnit();
            if (attacker is IBattleUser)
                return;

            attacker.UseAI();
        }
        private void SendAllReqDataToClient(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            IBattleUser user = _users.FirstOrDefault(y => y.SocketUser == socketUser);

            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Difficulity = _difficulity,
                Units = _allNpcs.Select(z => new BattleNpcUnitDTO
                {
                    AttackSpeed = z.Stats.AttackSpeed,
                    Health = z.Health,
                    Mana = z.Mana,
                    UniqueId = z.UnitData.UniqueId,
                    Damage = z.Stats.Damage,
                    MaxHealth = z.Stats.MaxHealth,
                    MaxMana = z.Stats.MaxMana,
                    IsDead = z.IsDeath,
                    UnitId = z.UnitId,
                    TeamIndex = z.UnitData.TeamIndex,
                    NodeIndex = _nodes.IndexOf(z.CurrentNode),
                    Skills = z.Skills.Select(v => v.GetSkillDataDTO()).ToArray()
                }).ToArray(),
                Players = _users.Select(z => new BattlePlayerDTO
                {
                    UniqueId = z.UnitData.UniqueId,
                    AttackSpeed = z.Stats.AttackSpeed,
                    Health = z.Health,
                    Mana = z.Mana,
                    Damage = z.Stats.Damage,
                    IsRealPlayer = z.SocketUser == socketUser,
                    PlayerName = z.PlayerName,
                    MaxHealth = z.Stats.MaxHealth,
                    MaxMana = z.Stats.MaxMana,
                    IsDead = z.IsDeath,
                    TeamIndex = z.UnitData.TeamIndex,
                    Skills = z.Skills.Select(v => v.GetSkillDataDTO()).ToArray(),
                    NodeIndex = _nodes.IndexOf(z.CurrentNode),
                }).ToArray()
            };

            SendToUser(user, BattleActions.LoadAll, loadData);
        }

    }
}
