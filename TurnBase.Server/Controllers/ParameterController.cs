using TurnBase.Server.ServerModels;
using TurnBase.Server.Services.Parameter;

namespace TurnBase.Server.Controllers
{
    public static class ParameterController
    {
        public static SocketResponse GetParameters(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ParameterService.ClientSendParameters);
        }
    }
}
