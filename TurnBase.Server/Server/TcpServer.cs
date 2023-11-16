using System.Net;
using System.Net.Sockets;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server
{
    public class TcpServer
    {
        public const bool LogUserAction = true;
        public const int BYTE_SIZE = 196000;
        public const string ENDFIX = "<EOL>";

        private TcpListener _tcpListener;
        private int _tcpPort;

        public TcpServer(int tcpPort)
        {
            _tcpPort = tcpPort;

            _tcpListener = new TcpListener(IPAddress.Any, tcpPort);
            _tcpListener.Start();

            _tcpListener.BeginAcceptSocket(OnUserConnect, null);
        }

        private void OnUserConnect(IAsyncResult ar)
        {
            try
            {
                Socket socket = _tcpListener.EndAcceptSocket(ar);
                SocketUser socketUser = new SocketUser(socket);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            finally
            {
                _tcpListener.BeginAcceptSocket(OnUserConnect, null);
            }
        }


        public static void WriteLog(string data, string error)
        {
            Console.WriteLine($"{DateTime.Now} - {data} -> {error}");
        }
    }
}
