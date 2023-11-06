using TurnBase.Server.Controllers;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server
{
    public static class ActionSelector
    {
        public static SocketResponse ExecuteAction(SocketUser socketUser, SocketRequest request)
        {
            using (SocketMethodParameter smp = new SocketMethodParameter(socketUser, request))
            {
                smp.LoadUserData();

                SocketResponse response = null;

                switch (request.Method)
                {
                    case Enums.ActionTypes.Login:
                        response = UserController.Login(smp);
                        break;
                    case Enums.ActionTypes.UserLevels:
                        response = UserLevelController.GetUserLevels(smp);
                        break;
                    case Enums.ActionTypes.StartBattle:
                        response = BattleController.StartABattle(smp);
                        break;
                    case Enums.ActionTypes.ExecuteBattleAction:
                        response = BattleController.ExecuteActionInBattle(smp);
                        break;
                    case Enums.ActionTypes.Ping:
                        response = PingController.Ping(smp);
                        break;
                    default:
                        Console.WriteLine("Methot Bulunamadı");
                        response = null;
                        break;
                }

                if (response != null && response.IsSuccess)
                {
                    smp.SendUsersToUnExpectedQueue();
                    smp.ExecuteOnSuccess();
                }

                return response;
            }
        }
    }
}
