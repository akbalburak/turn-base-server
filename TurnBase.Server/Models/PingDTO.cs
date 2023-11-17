using Newtonsoft.Json;

namespace TurnBase.Server.Models
{
    public struct PingDTO
    {
        [JsonProperty("A")] public long SendTime { get; set; }
    }
}
