using Newtonsoft.Json;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.DTO
{
    public class BattleActionResponseDTO
    {
        [JsonProperty("A")] public int RequestID { get; set; }
        [JsonProperty("B")] public BattleActions Action { get; set; }
        [JsonProperty("C")] public object? Data { get; set; }

        public BattleActionResponseDTO(BattleActionRequestDTO requestData, object data)
        {
            RequestID = requestData.Id;
            Action = requestData.BattleAction;
            Data = data.ToJson();
        }

        public BattleActionResponseDTO(int id, BattleActions battleAction, object data)
        {
            RequestID = id;
            Action = battleAction;
            Data = data.ToJson();
        }

        public static SocketResponse GetSuccess(int id, BattleActions battleAction, object data)
        {
            return new SocketResponse(ActionTypes.ExecuteBattleAction,
                new BattleActionResponseDTO(id, battleAction, data),
                true
            );
        }
    }
}
