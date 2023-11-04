using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleActionRequestDTO
    {
        [JsonProperty("A")] public int Id { get; set; }
        [JsonProperty("B")] public BattleActions BattleAction { get; set; }
        [JsonProperty("C")] public object? Data { get; set; }

        public T GetRequestData<T>()
        {
            if (Data == null)
                return (T)Activator.CreateInstance(typeof(T));
            return Data.ToObject<T>();
        }
    }

}
