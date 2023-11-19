using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Server.ServerModels
{
    public class SocketResponse
    {
        [JsonProperty("A")] public string RequestID { get; private set; }
        [JsonProperty("B")] public bool IsSuccess { get; private set; }
        [JsonProperty("C")] public ActionTypes Method { get; private set; }
        [JsonProperty("D")] public string? Data { get; private set; }
        [JsonProperty("E")] public string? Message { get; private set; }

        public SocketResponse(ISocketRequest request, bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
            SetRequest(request);
        }

        public SocketResponse(ActionTypes actionType, object data, bool isSuccess)
        {
            Method = actionType;
            Data = data.ToJson();
            IsSuccess = isSuccess;
            RequestID = $"{Guid.NewGuid()}";
        }

        public SocketResponse(object data, bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Data = data.ToJson();
            Message = message;
            RequestID = $"{Guid.NewGuid()}";
        }

        public void SetRequest(ISocketRequest request)
        {
            RequestID = request.RequestID;
            Method = request.Method;
        }

        public static SocketResponse GetSuccess()
        {
            return new SocketResponse(string.Empty, true, string.Empty);
        }
        public static SocketResponse GetSuccess(object data)
        {
            return new SocketResponse(data, true, string.Empty);
        }

        public static SocketResponse GetSuccess(ActionTypes method, object data)
        {
            return new SocketResponse(method, data, true);
        }

        public static SocketResponse GetError(string message)
        {
            return new SocketResponse(string.Empty, false, message);
        }
    }
}
