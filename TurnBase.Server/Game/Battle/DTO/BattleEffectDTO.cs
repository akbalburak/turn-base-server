using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleEffectTurnExecutionDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }
        [JsonProperty("C")] public int LeftTurnDuration { get; set; }
        [JsonProperty("D")] public int Damage { get; set; }

        public BattleEffectTurnExecutionDTO(IItemSkillEffect effect)
        {
            this.TargetUnitId = effect.ToWhom.UniqueId;
            this.Effect = effect.Effect;
            this.LeftTurnDuration = effect.LeftTurnDuration;
        }
    }

    public class BattleEffectStartedDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }
        [JsonProperty("C")] public int OwnerUnitId { get; set; }
        [JsonProperty("D")] public int LeftTurnDuration { get; set; }

        public BattleEffectStartedDTO(IItemSkillEffect effect)
        {
            this.Effect = effect.Effect;
            this.TargetUnitId = effect.ToWhom.UniqueId;
            this.OwnerUnitId = effect.ByWhom.UniqueId;
            this.LeftTurnDuration = effect.LeftTurnDuration;
        }
    }

    public class BattleEffectOverDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }

        public BattleEffectOverDTO(IItemSkillEffect effect)
        {
            this.Effect = effect.Effect;
            this.TargetUnitId = effect.ToWhom.UniqueId;
        }
    }
}
