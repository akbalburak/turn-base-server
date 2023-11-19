using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Services
{
    public static class SocketUserServices
    {
        private static List<ISocketUser> _socketUsers = new List<ISocketUser>();

        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();

        public static void Initialize()
        {
            SocketUserBusSystem.OnSocketUserConnect += OnUserLogin;
            SocketUserBusSystem.OnSocketUserDisconnect += OnUserLogout;
        }

        private static void OnUserLogout(ISocketUser user)
        {
            _rwls.EnterWriteLock();
            _socketUsers.Remove(user);
            _rwls.ExitWriteLock();
        }

        private static void OnUserLogin(ISocketUser user)
        {
            _rwls.EnterWriteLock();
            _socketUsers.Insert(0, user);
            _rwls.ExitWriteLock();
        }

        public static ISocketUser GetSocketUser(int id)
        {
            _rwls.EnterReadLock();
            using ISocketUser user = _socketUsers.Find(y => y.User?.Id == id);
            _rwls.ExitReadLock();
            return user;
        }
    }
}
