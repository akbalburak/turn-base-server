﻿using TurnBase.Server.ServerModels;
using TurnBase.Server.Services;

namespace TurnBase.Server.Controllers
{
    public static class UserLevelController
    {
        public static SocketResponse GetUserLevels(SocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(UserLevelService.UserLevels);
        }
    }
}
