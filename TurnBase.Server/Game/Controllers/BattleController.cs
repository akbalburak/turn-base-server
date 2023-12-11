using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class BattleController
    {
        // TEMPORARY TO TEST MULTIPLAYER.
        private static BattleUser _waitingUser;
        public static SocketResponse StartABattle(ISocketMethodParameter smp)
        {
            if (smp.SocketUser.CurrentBattle != null)
                return SocketResponse.GetError("YOU ARE ALREADY IN A MATCH!");

            BattleDTO.BattleRequestDTO requestData = smp
                .GetRequestData<BattleDTO.BattleRequestDTO>();

            // WE GET USER DATA.
            TrackedUser user = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);
            CampaignDTO campaign = user.GetCampaign();
            InventoryDTO inventory = user.GetInventory();
            IEquipmentItemDTO[] equipments = inventory.GetEquippedItems();

            // WE CHECK IF WE ARE GOING TO GIVE USER THE LEVEL REWARDS.
            bool isFirstCompletion = !campaign.IsLevelAlreadyCompleted(
                requestData.StageIndex,
                requestData.LevelIndex);

            // WE CREATE A BATTLE USER.
            BattleUser battleUser = new BattleUser(
                socketUser: smp.SocketUser,
                equipments: equipments,
                isFirstCompletion: isFirstCompletion
            );

            if (_waitingUser == null)
            {
                _waitingUser = battleUser;
                return SocketResponse.GetSuccess();
            }

            // WE CREATE A CAMPAIGN LEVEL.
            BattleService.CreateALevel(new IBattleUser[]
                { battleUser, _waitingUser },
                requestData.StageIndex,
                requestData.LevelIndex
            );

            return SocketResponse.GetSuccess();
        }

        public static SocketResponse ExecuteActionInBattle(ISocketMethodParameter smp)
        {
            if (smp.SocketUser.CurrentBattle == null)
                return SocketResponse.GetError("BATTLE NOT FOUND!");

            BattleActionRequestDTO actionData = smp.GetRequestData<BattleActionRequestDTO>();
            smp.SocketUser.CurrentBattle.ExecuteAction(smp.SocketUser, actionData);

            return SocketResponse.GetSuccess();
        }
    }
}
