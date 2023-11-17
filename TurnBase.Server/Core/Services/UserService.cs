using TurnBase.DBLayer.Models;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Trackables;

namespace TurnBase.Server.Core.Services
{
    public static class UserService
    {
        public static TrackedUser GetTrackedUser(SocketMethodParameter smp, long userId)
        {
            TblUser user = smp.UOW.GetRepository<TblUser>().Find(y => y.Id == userId);
            if (user == null)
                return null;

            return new TrackedUser(user, smp);
        }
    }
}
