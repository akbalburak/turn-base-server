using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleSkillDTO
    {
        [JsonProperty("A")] public int UniqueID { get; set; }
        [JsonProperty("B")] public BattleSkills Skill { get; set; }
        [JsonProperty("C")] public int LeftTurnToUse { get; set; }
        [JsonProperty("D")] public int TurnCooldown { get; set; }
    }
}
