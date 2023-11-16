using TurnBase.Server.Core.Services;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
{
    public static class ParameterController
    {
        public static SocketResponse GetParameters(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ParameterService.ClientSendParameters);
        }
    }
}
