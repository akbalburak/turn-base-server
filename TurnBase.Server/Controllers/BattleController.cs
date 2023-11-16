using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Battle;
using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.ServerModels;
using TurnBase.Server.Services;

namespace TurnBase.Server.Controllers
{
    public static class BattleController
    {
        public static SocketResponse StartABattle(SocketMethodParameter smp)
        {
            if (smp.SocketUser.CurrentBattle != null && !smp.SocketUser.CurrentBattle.IsDisposed)
                return SocketResponse.GetError("YOU ARE ALREADY IN A MATCH!");

            BattleDTO.BattleRequestDTO requestData = smp
                .GetRequestData<BattleDTO.BattleRequestDTO>();

            // WE GET USER DATA.
            TblUser user = smp.UOW.GetRepository<TblUser>()
                .Find(y => y.Id == smp.SocketUser.User.Id);

            // WE CHECK IF WE ARE GOING TO GIVE USER THE LEVEL REWARDS.
            CampaignDTO campaign = user.GetCampaign();
            bool isFirstCompletion = !campaign.IsDifficulityCompleted(
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity);

            // WE LOAD ALL THE STATS OF THE PLAYER.
            UnitStats userStats = new UnitStats();
            userStats.SetUser(user);

            // WE CREATE A CAMPAIGN LEVEL.
            BattleService.CreateALevel(new BattleUser[]
                {
                    new BattleUser(

                        socketUser: smp.SocketUser,
                        playerName: smp.SocketUser.User.UserName,
                        position: 0,
                        stats:userStats,
                        isFirstCompletion:isFirstCompletion
                    )
                },
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
