using Newtonsoft.Json;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleAttackUseDTO
    {
        [JsonProperty("A")] public int TargetUniqueId { get; private set; }
    }
}
