using TurnBase.Server.Server;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Services
{
    public static class SocketUserBusSystem
    {
        public static Action<ISocketUser> OnSocketUserConnect;
        public static void CallSocketUserConnect(ISocketUser user)
        {
            TcpServer.WriteLog(null, "A User Connected");
            OnSocketUserConnect?.Invoke(user);
        }

        public static Action<ISocketUser> OnSocketUserDisconnect;
        public static void CallSocketUserDisconnect(ISocketUser user)
        {
            TcpServer.WriteLog(null, "A User Disconnected");
            OnSocketUserDisconnect?.Invoke(user);
        }

    }
}
