using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Services
{
    public static class SocketUserServices
    {
        private static List<SocketUser> _socketUsers = new List<SocketUser>();

        private static ReaderWriterLockSlim _rwls = new ReaderWriterLockSlim();

        public static void Initialize()
        {
            SocketUserBusSystem.OnSocketUserConnect += OnUserLogin;
            SocketUserBusSystem.OnSocketUserDisconnect += OnUserLogout;
        }

        private static void OnUserLogout(SocketUser user)
        {
            _rwls.EnterWriteLock();
            _socketUsers.Remove(user);
            _rwls.ExitWriteLock();
        }

        private static void OnUserLogin(SocketUser user)
        {
            _rwls.EnterWriteLock();
            _socketUsers.Insert(0, user);
            _rwls.ExitWriteLock();
        }

        public static SocketUser GetSocketUser(int id)
        {
            _rwls.EnterReadLock();
            using SocketUser user = _socketUsers.Find(y => y.User?.Id == id);
            _rwls.ExitReadLock();
            return user;
        }
    }
}
