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
            if (smp.SocketUser.CurrentBattle != null)
                return SocketResponse.GetError("YOU ARE ALREADY IN A MATCH!");

            BattleDTO.BattleRequestDTO requestData = smp
                .GetRequestData<BattleDTO.BattleRequestDTO>();

            BattleService.CreateALevel(new BattleUser[]
                {
                    new BattleUser(
                        user: smp.SocketUser,
                        playerName: smp.SocketUser.User.UserName,
                        health: 50,
                        position: 0,
                        minDamage: 1,
                        maxDamage: 3,
                        attackSpeed: 1
                    )
                }, 
                requestData.StageIndex, 
                requestData.LevelIndex
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
