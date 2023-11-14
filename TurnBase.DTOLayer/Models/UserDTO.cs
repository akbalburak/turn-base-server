using Newtonsoft.Json;

namespace TurnBase.DTOLayer.Models
{
    public class UserDTO
    {
        [JsonProperty("A")] public long Id { get; set; }
        [JsonProperty("B")] public string Username { get; set; }
        [JsonProperty("C")] public int UserLevel { get; set; }
        [JsonProperty("D")] public int Experience { get; set; }
        [JsonProperty("E")] public long Gold { get; set; }
        [JsonProperty("F")] public string Inventory { get; set; }
        [JsonProperty("G")] public string Campaign { get; set; }
    }
}
