using TurnBase.Server.Enums;

namespace TurnBase.Server.ServerModels
{
    public class SocketRequest
    {
        public string RequestID { get; set; }
        public ActionTypes Method { get; set; }
        public object Data { get; set; }
    }
}
