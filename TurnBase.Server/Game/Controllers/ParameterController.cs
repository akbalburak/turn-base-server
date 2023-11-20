using TurnBase.Server.Game.Services;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class ParameterController
    {
        public static SocketResponse GetParameters(ISocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ParameterService.ClientSendParameters);
        }
    }
}
