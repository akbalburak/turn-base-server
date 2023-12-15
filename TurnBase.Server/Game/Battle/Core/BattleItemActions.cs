using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void ExecuteAction(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            // IF THE GAME IS OVER NO LONGER ACTIONS CAN BE EXECUTED.
            if (GameOver)
                return;

            switch (requestData.BattleAction)
            {
                case BattleActions.LoadAll:
                    SendAllGameData(socketUser, requestData);
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
                case BattleActions.ClaimADrop:
                    ClaimDrop(socketUser, requestData);
                    break;
                case BattleActions.LeaveBattle:
                    LeaveBattle(socketUser);
                    break;
            }
        }

        private void LeaveBattle(ISocketUser socketUser)
        {
            socketUser.ClearBattle();

            // WE GET THE USER.
            IBattleUser actionUser = GetUser(socketUser);
            if (actionUser.IsConnected == false)
                return;

            // WE SET AS DISCONNECTED.
            actionUser.SetAsDisconnected();

            // IF ALL USERS LEFT WE WILL TERMINATE THE GAME.
            if (_users.All(x => x.IsConnected == false))
            {
                // WE FINALIZE GAME FOR ALL USERS.
                FinalizeBattle(isVictory: false);
                Dispose();
            }
        }

        private void ClaimDrop(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            BattleDropClaimRequestDTO data = requestData.GetRequestData<BattleDropClaimRequestDTO>();

            lock (_drops)
            {
                IBattleUser user = GetUser(socketUser);

                // WE FIND PLAYER DROP FOR THE GIVEN UNIT.
                IBattleDrop drop = _drops.Find(x =>
                    x.DropOwner == user &&
                    x.KilledUnit.UnitData.UniqueId == data.UnitUniqueId
                );

                if (drop == null)
                    return;

                drop.Claim(data.DropItemId);
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
            IBattleUser user = GetUser(socketUser);
            if (user.IsReady) return;

            user.SetAsReady();
            _turnHandler.AddUnits(new IBattleUnit[] { user });

            if (_gameStarted)
                return;

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

            currentUser.UseSkill(useData);
        }
        private void SendAllGameData(ISocketUser socketUser, BattleActionRequestDTO requestData)
        {
            IBattleUser user = GetUser(socketUser);
            IBattleUnit currentTurnUnit = _turnHandler.GetCurrentTurnUnit();

            // WE GET PLAYER DROPS.
            IBattleDrop[] drops = Array.Empty<IBattleDrop>();
            lock (_drops)
                drops = _drops.Where(x => x.DropOwner == user).ToArray();

            // ALL THE REQUIRED DATA TO CONTINUE GAME.
            BattleLoadAllDTO loadData = new BattleLoadAllDTO()
            {
                Stage = _levelData.Stage,
                Level = _levelData.Level,
                IsInCombat = _turnHandler.IsInCombat,
                Units = _allNpcs.Select(npc => new BattleNpcUnitDTO(this, npc)).ToArray(),
                Players = _users.Select(user => new BattlePlayerDTO(this, user, user.SocketUser == socketUser)).ToArray(),
                LastDataId = user.GetLastDataId,
                TurnData = currentTurnUnit == null ? null : new BattleTurnDTO(currentTurnUnit.UnitData.UniqueId),
                Drops = drops.Select(x => new BattleDropDTO(x)).ToArray(),
                LootInventory = user.LootInventory.IItems.Select(x => new BattleInventoryDTO(x)).ToArray()
            };

            SendToUser(user, BattleActions.LoadAll, loadData);

            _turnHandler.CalculateAttackOrder();
        }


        private void TryPlayAITurn()
        {
            // WE LOOP TILL PLAYER TURN.
            IBattleUnit attacker = _turnHandler.GetCurrentTurnUnit();

            // WE CHECK IF PLAYERS WE FINALIZE THE TURN.
            if (attacker is IBattleUser battleUser)
            {
                // THER SHOULD BE ATLEAST ONE PLAYER CAN PLAY.
                if (_users.Count(x => x.IsConnected) == 0)
                    return;

                // IF PLAYER NOT CONNECTED WE USE AI FOR IT.
                if (battleUser.IsConnected)
                    return;
            }

            attacker.UseAI();
        }

        public void ReConnectUser(ISocketUser socketUser)
        {
            var existsUser = _users.FirstOrDefault(x => x.SocketUser.User.Id == socketUser.User.Id);
            if (existsUser == null)
                return;

            existsUser.UpdateSocketUser(socketUser);
        }
    }
}
