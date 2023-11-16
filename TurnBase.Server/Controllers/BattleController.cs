﻿using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Battle;
using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Services;
using TurnBase.Server.ServerModels;

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

            TblUser user = smp.UOW.GetRepository<TblUser>()
                .Find(y => y.Id == smp.SocketUser.User.Id);

            CampaignDTO campaign = user.GetCampaign();
            bool isFirstCompletion = !campaign.IsDifficulityCompleted(
                requestData.StageIndex,
                requestData.LevelIndex,
                requestData.Difficulity);

            Battle.Models.UnitStats userStats = new Battle.Models.UnitStats();
            userStats.SetUser(user);

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
    }
}
