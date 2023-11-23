using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillDTO
    {
        [JsonProperty("A")] public int UniqueID { get; set; }
        [JsonProperty("B")] public Enums.ItemSkills Skill { get; set; }
        [JsonProperty("C")] public int LeftTurnToUse { get; set; }
        [JsonProperty("D")] public int TurnCooldown { get; set; }
        [JsonProperty("E")] public int UsageManaCost { get; set; }
    }
}
