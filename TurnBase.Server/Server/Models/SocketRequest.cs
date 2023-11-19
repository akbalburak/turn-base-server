using TurnBase.Server.Enums;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Server.ServerModels
{
    public class SocketRequest : ISocketRequest
    {
        public string RequestID { get; set; }
        public ActionTypes Method { get; set; }
        public object Data { get; set; }
    }
}
