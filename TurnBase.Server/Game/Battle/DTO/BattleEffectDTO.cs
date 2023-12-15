using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Enums;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleEffectTurnExecutionDTO : BaseBattleEffectDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }
        [JsonProperty("C")] public int LeftTurnDuration { get; set; }

        public BattleEffectTurnExecutionDTO(IItemSkillEffect effect) : base(effect)
        {
            this.TargetUnitId = effect.ToWhom.UnitData.UniqueId;
            this.Effect = effect.Effect;
            this.LeftTurnDuration = effect.LeftTurnDuration;
        }
    }

    public class BattleEffectStartedDTO : BaseBattleEffectDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }
        [JsonProperty("C")] public int OwnerUnitId { get; set; }
        [JsonProperty("D")] public int LeftTurnDuration { get; set; }
        [JsonProperty("E")] public bool IsFriendEffect { get; set; }

        public BattleEffectStartedDTO(IItemSkillEffect effect) : base(effect)
        {
            this.IsFriendEffect = effect.IsFriendEffect;
            this.Effect = effect.Effect;
            this.TargetUnitId = effect.ToWhom.UnitData.UniqueId;
            this.OwnerUnitId = effect.ByWhom.UnitData.UniqueId;
            this.LeftTurnDuration = effect.LeftTurnDuration;
        }


    }

    public class BattleEffectOverDTO : BaseBattleEffectDTO
    {
        [JsonProperty("A")] public BattleEffects Effect { get; set; }
        [JsonProperty("B")] public int TargetUnitId { get; set; }

        public BattleEffectOverDTO(IItemSkillEffect effect) : base(effect)
        {
            this.Effect = effect.Effect;
            this.TargetUnitId = effect.ToWhom.UnitData.UniqueId;
        }
    }

    public abstract class BaseBattleEffectDTO
    {
        [JsonProperty("Z")] public Dictionary<ItemSkillEffectAttributes, object> Attributes { get; private set; }
        public BaseBattleEffectDTO(IItemSkillEffect effect)
        {
            this.Attributes = new Dictionary<ItemSkillEffectAttributes, object>();

            foreach (var attribute in effect.GetTempAttributes())
                Attributes.Add(attribute.Key, attribute.Value);
        }

        public T GetAttribute<T>(ItemSkillEffectAttributes attribute)
        {
            if (!Attributes.TryGetValue(attribute,out object value))
                return default;
            return (T)value;
        }
    }
}
