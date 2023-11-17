using Newtonsoft.Json;

namespace TurnBase.Server.Models
{
    public class UserLevelDTO
    {
        [JsonProperty("A")] public int Level { get; set; }
        [JsonProperty("B")] public int Experience { get; set; }
    }
}
