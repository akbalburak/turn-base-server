using TurnBase.Server.Enums;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketRequest
    {
        object Data { get; }
        ActionTypes Method { get; }
        string RequestID { get; }
    }
}
