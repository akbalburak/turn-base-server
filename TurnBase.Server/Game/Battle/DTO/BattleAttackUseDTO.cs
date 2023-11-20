using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleAttackUseDTO
    {
        [JsonProperty("A")] public int TargetUniqueId { get; private set; }
    }
}
