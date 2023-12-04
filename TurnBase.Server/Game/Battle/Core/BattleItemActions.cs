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
                    StartGame(socketUser);
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

            FinalizeTurn();
        }

        private void StartGame(ISocketUser socketUser)
        {
            if (_gameStarted)
                return;

            IBattleUser user = GetUser(socketUser);

            _turnHandler.AddUnits(new IBattleUnit[] { user });

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
        private void TryPlayAITurn()
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
                Units = _allNpcs.Select(npc => new BattleNpcUnitDTO(this, npc)).ToArray(),
                Players = _users.Select(user => new BattlePlayerDTO(this, user, user.SocketUser == socketUser)).ToArray(),
                LastDataId = user.GetLastDataId
            };

            SendToUser(user, BattleActions.LoadAll, loadData);

            _turnHandler.CalculateAttackOrder();
        }

    }
}
