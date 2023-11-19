using TurnBase.Server.Core.Services;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
{
    public static class ParameterController
    {
        public static SocketResponse GetParameters(ISocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ParameterService.ClientSendParameters);
        }
    }
}
