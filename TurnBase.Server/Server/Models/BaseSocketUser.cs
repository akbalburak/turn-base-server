using System.Net.Sockets;
using System.Text;

namespace TurnBase.Server.Server.ServerModels
{
    public abstract class BaseSocketUser : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public byte[] ReadBytes { get; private set; }
        public string PreviousTcpData { get; private set; }
        public Socket Socket { get; private set; }
        public DateTime LastPacketDate { get; private set; }

        public BaseSocketUser(Socket socket)
        {
            Socket = socket;
            ReadBytes = new byte[TcpServer.BYTE_SIZE];
            PreviousTcpData = string.Empty;
            LastPacketDate = DateTime.UtcNow;

            socket.SendTimeout = -1;

            socket.BeginReceive(ReadBytes, 0, ReadBytes.Length,
                SocketFlags.None, new AsyncCallback(OnReceiveData), this);
        }

        private void OnReceiveData(IAsyncResult ar)
        {
            try
            {
                // IF USER ALREADY DISPOSED.
                BaseSocketUser socketUser = (BaseSocketUser)ar.AsyncState;
                if (socketUser.IsDisposed)
                    return;

                // DISCONNECT.
                int dataLength = socketUser.Socket.EndReceive(ar);
                if (dataLength <= 0)
                {
                    socketUser.Dispose();
                    return;
                }

                string data = Encoding.UTF8.GetString(socketUser.ReadBytes, 0, dataLength);

                ThreadPool.QueueUserWorkItem((t) => AddData(data));

                socketUser.Socket.BeginReceive(socketUser.ReadBytes, 0, socketUser.ReadBytes.Length,
                    SocketFlags.None, new AsyncCallback(OnReceiveData), socketUser);
            }
            catch (SocketException exc)
            {
                if (exc.SocketErrorCode != SocketError.ConnectionReset)
                    TcpServer.WriteLog(string.Empty, exc.ToString());
            }
        }


        private void AddData(string data)
        {
            LastPacketDate = DateTime.UtcNow;

            lock (this)
            {
                if (!data.EndsWith(TcpServer.ENDFIX))
                {
                    PreviousTcpData += data;
                    return;
                }
                else
                {
                    data = PreviousTcpData + data;
                    PreviousTcpData = string.Empty;
                }

                OnAddData(data);
            }
        }

        protected abstract void OnAddData(string data);

        public virtual void Dispose()
        {
            try
            {
                lock (this)
                {
                    if (IsDisposed)
                        return;

                    IsDisposed = true;

                    if (Socket != null)
                    {
                        Socket.Close();
                        Socket.Dispose();
                    }

                    OnDispose();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }
        public abstract void OnDispose();
    }
}
