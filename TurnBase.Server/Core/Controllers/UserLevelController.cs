using TurnBase.Server.Core.Services;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
{
    public static class UserLevelController
    {
        public static SocketResponse GetUserLevels(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(UserLevelService.UserLevels);
        }
    }
}
