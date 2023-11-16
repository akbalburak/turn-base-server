using Newtonsoft.Json;

namespace TurnBase.Server.Core.Battle.DTO
{
    public class BattleSkillUseDTO
    {
        [JsonProperty("A")] public int UniqueSkillID { get; set; }
        [JsonProperty("B")] public int TargetUnitID { get; set; }
    }
}
