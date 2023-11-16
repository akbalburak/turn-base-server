using TurnBase.Server.Server;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Services
{
    public static class SocketUserBusSystem
    {
        public static Action<SocketUser> OnSocketUserConnect;
        public static void CallSocketUserConnect(SocketUser user)
        {
            TcpServer.WriteLog(null, "A User Connected");
            OnSocketUserConnect?.Invoke(user);
        }

        public static Action<SocketUser> OnSocketUserDisconnect;
        public static void CallSocketUserDisconnect(SocketUser user)
        {
            TcpServer.WriteLog(null, "A User Disconnected");
            OnSocketUserDisconnect?.Invoke(user);
        }

    }
}
