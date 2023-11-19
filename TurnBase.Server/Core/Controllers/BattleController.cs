using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Battle.Skills;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Trackables;

namespace TurnBase.Server.Core.Controllers
{
    public static class BattleController
    {
        public static SocketResponse StartABattle(SocketMethodParameter smp)
        {
            if (smp.SocketUser.CurrentBattle != null)
                return SocketResponse.GetError("YOU ARE ALREADY IN A MATCH!");

            BattleDTO.BattleRequestDTO requestData = smp
                .GetRequestData<BattleDTO.BattleRequestDTO>();

            // WE GET USER DATA.
            TrackedUser user = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);

            // WE CHECK IF WE ARE GOING TO GIVE USER THE LEVEL REWARDS.
            CampaignDTO campaign = user.GetCampaign();
            InventoryDTO inventory = user.GetInventory();

            bool isFirstCompletion = !campaign.IsDifficulityCompleted(
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity);

            // WE LOAD ALL THE STATS OF THE PLAYER.
            UnitStats userStats = new UnitStats();
            userStats.SetInventory(inventory);

            BattleUser battleUser = new BattleUser(
                socketUser: smp.SocketUser,
                inventory: inventory,
                unitStats: userStats,
                position: 0,
                isFirstCompletion: isFirstCompletion
            );

            // WE CREATE A CAMPAIGN LEVEL.
            BattleService.CreateALevel(new BattleUser[]
                { battleUser },
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity
            );

            return SocketResponse.GetSuccess();
        }

        public static SocketResponse ExecuteActionInBattle(SocketMethodParameter smp)
        {
            if (smp.SocketUser.CurrentBattle == null)
                return SocketResponse.GetError("BATTLE NOT FOUND!");

            BattleActionRequestDTO actionData = smp.GetRequestData<BattleActionRequestDTO>();
            smp.SocketUser.CurrentBattle.ExecuteAction(smp.SocketUser, actionData);

            return SocketResponse.GetSuccess();
        }

        public static SocketResponse GetBattleRewards(SocketMethodParameter smp)
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
