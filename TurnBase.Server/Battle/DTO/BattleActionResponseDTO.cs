using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleActionResponseDTO
    {
        [JsonProperty("A")] public int RequestID { get; set; }
        [JsonProperty("B")] public BattleActions Action { get; set; }
        [JsonProperty("C")] public object? Data { get; set; }

        public BattleActionResponseDTO(BattleActionRequestDTO requestData, object data)
        {
            this.RequestID = requestData.Id;
            this.Action = requestData.BattleAction;
            this.Data = data.ToJson();
        }

        public BattleActionResponseDTO(int id, BattleActions battleAction, object data)
        {
            this.RequestID = id;
            this.Action = battleAction;
            this.Data = data.ToJson();
        }

        public static SocketResponse GetSuccess(int id, BattleActions battleAction, object data)
        {
            return new SocketResponse(ActionTypes.ExecuteBattleAction, 
                new BattleActionResponseDTO(id,battleAction,data), 
                true
            );
        }
    }
}
