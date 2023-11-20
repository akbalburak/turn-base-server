using TurnBase.Server.Game.DTO;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class PingController
    {
        public static SocketResponse Ping(ISocketMethodParameter smp)
        {
            PingDTO pingData = smp.GetRequestData<PingDTO>();
            return SocketResponse.GetSuccess(pingData);
        }
    }
}
