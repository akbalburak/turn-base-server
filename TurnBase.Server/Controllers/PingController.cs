using TurnBase.DTOLayer.Models;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Controllers
{
    public static class PingController
    {
        public static SocketResponse Ping(SocketMethodParameter smp)
        {
            PingDTO pingData = smp.GetRequestData<PingDTO>();
            return SocketResponse.GetSuccess(pingData);
        }
    }
}
