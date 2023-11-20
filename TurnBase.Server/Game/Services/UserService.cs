using TurnBase.DBLayer.Models;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Services
{
    public static class UserService
    {
        public static TrackedUser GetTrackedUser(ISocketMethodParameter smp, long userId)
        {
            TblUser user = smp.UOW.GetRepository<TblUser>().Find(y => y.Id == userId);
            if (user == null)
                return null;

            return new TrackedUser(user, smp);
        }
    }
}
