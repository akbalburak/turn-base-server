﻿using Newtonsoft.Json;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.ServerModels
{
    public class SocketResponse
    {
        [JsonProperty("A")] public string RequestID { get; private set; }
        [JsonProperty("B")] public bool IsSuccess { get; private set; }
        [JsonProperty("C")] public ActionTypes Method { get; private set; }
        [JsonProperty("D")] public string? Data { get; private set; }
        [JsonProperty("E")] public string? Message { get; private set; }

        public SocketResponse(SocketRequest request, bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            SetRequest(request);
        }

        public SocketResponse(ActionTypes actionType, object data, bool isSuccess)
        {
            this.Method = actionType;
            this.Data = data.ToJson();
            this.IsSuccess = isSuccess;
            this.RequestID = $"{Guid.NewGuid()}";
        }

        public SocketResponse(object data, bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Data = data.ToJson();
            this.Message = message;
            this.RequestID = $"{Guid.NewGuid()}";
        }

        public void SetRequest(SocketRequest request)
        {
            this.RequestID = request.RequestID;
            this.Method = request.Method;
        }

        #region Başarılı Mesajları

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

        #endregion

        #region Hata mesajları

        public static SocketResponse GetError(object data)
        {
            return new SocketResponse(data, false, string.Empty);
        }

        public static SocketResponse GetError(object data, string message)
        {
            return new SocketResponse(data, false, message);
        }

        public static SocketResponse GetError(string message)
        {
            return new SocketResponse(string.Empty, false, message);
        }

        #endregion
    }
}
