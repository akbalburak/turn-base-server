using System.Diagnostics;
using System.Net.Sockets;
using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Server.Services;

namespace TurnBase.Server.Server.ServerModels
{
    public class SocketUser : BaseSocketUser
    {
        public const int ListSize = 25;
        public const int TimeOutSeconds = 45;

        public Guid TempUniqueUserID { get; }
        public SocketUserData User { get; }
        public BattleItem CurrentBattle { get; private set; }

        private List<SocketResponse> _unExpectedNotReceivedResponses;
        private List<Tuple<SocketRequest, byte[]>> _waitingResponses;

        private List<SocketRequest> _waitingRequests;

        private Timer _unExpectedTimer;

        public SocketUser(Socket socket) : base(socket)
        {
            TempUniqueUserID = Guid.NewGuid();
            User = new SocketUserData();

            _waitingRequests = new List<SocketRequest>();
            _waitingResponses = new List<Tuple<SocketRequest, byte[]>>();

            _unExpectedNotReceivedResponses = new List<SocketResponse>();

            _unExpectedTimer = new Timer(new TimerCallback(OnUnExpectedTimerElapsed), this, 2000, 2000);

            SocketUserBusSystem.CallSocketUserConnect(this);
        }

        public void SetBattle(BattleItem battle)
        {
            CurrentBattle = battle;
        }

        public void AddToUnExpectedAfterSendIt(SocketResponse response)
        {
            if (IsDisposed)
                return;

            lock (_unExpectedNotReceivedResponses)
            {
                SendToClient(response.ToByteArray());
                _unExpectedNotReceivedResponses.Add(response);
            }
        }

        private void OnUnExpectedTimerElapsed(object state)
        {
            if (IsDisposed)
                return;

            // IF USER TIMEOUTS.
            if ((DateTime.UtcNow - LastPacketDate).TotalSeconds >= TimeOutSeconds)
            {
                Dispose();
                return;
            }

            if (_unExpectedNotReceivedResponses.Count == 0)
                return;

            lock (_unExpectedNotReceivedResponses)
            {
                foreach (SocketResponse response in _unExpectedNotReceivedResponses)
                    SendToClient(response.ToByteArray());
            }
        }

        protected override void OnAddData(string data)
        {
            string[] jsonValues = data.Split(TcpServer.ENDFIX, StringSplitOptions.RemoveEmptyEntries);

            foreach (string json in jsonValues)
            {
                try
                {
                    SocketRequest request = json.ToObject<SocketRequest>();

                    // IF THIS IS A VERIFICATION WE JUST REMOVE FROM LIST.
                    if (request.Method == ActionTypes.RA)
                    {
                        lock (_unExpectedNotReceivedResponses)
                        {
                            SocketResponse unExpectedData = _unExpectedNotReceivedResponses.Find(x => x.RequestID == request.RequestID);

                            if (unExpectedData != null)
                                _unExpectedNotReceivedResponses.Remove(unExpectedData);

                            continue;
                        }
                    }

                    lock (_waitingResponses)
                    {
                        // IF THIS REQUEST ALREADY EXECUTED.
                        Tuple<SocketRequest, byte[]> alreadyExecutedData = _waitingResponses.Find(x => x.Item1.RequestID == request.RequestID);
                        if (alreadyExecutedData != null)
                        {
                            SendToClient(alreadyExecutedData.Item2);
                            continue;
                        }

                        // WE EXECUTE THE ACTION.
                        lock (_waitingRequests)
                        {
                            bool isAlreadyAddedToQueue = _waitingRequests.Exists(x => x != null && x.RequestID == request.RequestID);

                            if (!isAlreadyAddedToQueue)
                            {
                                _waitingRequests.Add(request);

                                ActionExecuter(request);
                            }
                        }

                    }
                }
                catch (Exception exc)
                {
                    TcpServer.WriteLog(json, exc.ToString());
                }
            }

            if (_waitingRequests.Count > ListSize)
                _waitingRequests.RemoveRange(ListSize, _waitingRequests.Count - ListSize);
        }

        public void ActionExecuter(SocketRequest request)
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                SocketResponse response;

                lock (this)
                {
                    response = ActionSelector.ExecuteAction(this, request);
                    response.SetRequest(request);
                }

                sw.Stop();

                TcpServer.WriteLog(User?.UserName, $"{request.Method} -> {sw.ElapsedMilliseconds}ms");

                byte[] responseBytes = response.ToByteArray();

                lock (_waitingResponses)
                {
                    _waitingResponses.Insert(0, new Tuple<SocketRequest, byte[]>(request, responseBytes));

                    if (_waitingResponses.Count > ListSize)
                    {
                        int excessCount = _waitingResponses.Count - ListSize;
                        _waitingResponses.RemoveRange(ListSize, excessCount);
                    }
                }

                SendToClient(responseBytes);

                if (!response.IsSuccess && User != null)
                    TcpServer.WriteLog(request.ToJson(), response.Message);

            }
            catch (Exception exc)
            {
                string error = exc.ToString();
                TcpServer.WriteLog(request.ToJson(), exc.ToString());

                try
                {
                    SocketResponse response = new SocketResponse(request, false, error);

                    byte[] responseBytes = response.ToByteArray();

                    lock (_waitingResponses)
                        _waitingResponses.Add(new Tuple<SocketRequest, byte[]>(request, responseBytes));

                    Socket.Send(responseBytes);

                }
                catch (Exception exc2)
                {
                    Console.WriteLine(exc2);
                }
            }
        }

        public void SendToClient(byte[] data)
        {
            try
            {
                if (!IsDisposed && Socket != null && Socket.Connected)
                    Socket.Send(data);
            }
            catch (SocketException exc)
            {
                TcpServer.WriteLog(null, $"{exc.ErrorCode}");
            }
        }

        public override void OnDispose()
        {
            try
            {
                lock (this)
                {
                    _unExpectedTimer.Dispose();

                    SocketUserBusSystem.CallSocketUserDisconnect(this);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }
    }
}
