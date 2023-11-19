using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
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
