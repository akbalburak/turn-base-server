using Newtonsoft.Json;

namespace TurnBase.Server.Game.DTO
{
    public struct PingDTO
    {
        [JsonProperty("A")] public long SendTime { get; set; }
    }
}
