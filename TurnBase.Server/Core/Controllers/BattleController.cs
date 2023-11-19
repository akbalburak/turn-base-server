using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Trackables;

namespace TurnBase.Server.Core.Controllers
{
    public static class BattleController
    {
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

            // WE CHECK IF WE ARE GOING TO GIVE USER THE LEVEL REWARDS.
            bool isFirstCompletion = !campaign.IsDifficulityCompleted(
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity);

            // WE CREATE A BATTLE USER.
            BattleUser battleUser = new BattleUser(
                socketUser: smp.SocketUser,
                inventory: inventory,
                position: 0,
                isFirstCompletion: isFirstCompletion
            );

            // WE CREATE A CAMPAIGN LEVEL.
            BattleService.CreateALevel(new IBattleUser[]
                { battleUser },
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity
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

        public static SocketResponse GetBattleRewards(ISocketMethodParameter smp)
        {
            BattleDTO.BattleDataRequestDTO requestData = smp
                .GetRequestData<BattleDTO.BattleDataRequestDTO>();

            BattleDTO.BattleDataResponseDTO levelData = BattleLevelService.GetLevelMetaData(
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity
            );

            if (levelData == null)
                return SocketResponse.GetSuccess(new BattleDTO.BattleDataResponseDTO());
            else
                return SocketResponse.GetSuccess(levelData);
        }
    }
}
