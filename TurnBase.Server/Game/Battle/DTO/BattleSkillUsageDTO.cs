using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.ItemSkills.Enums;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillUsageDTO
    {
        [JsonProperty("A")] public int UniqueSkillId { get; private set; }
        [JsonProperty("B")] public Enums.ItemSkills ItemSkill { get; private set; }
        [JsonProperty("C")] public int SkillOwnerId { get; private set; }
        [JsonProperty("D")] public bool FinalizeTurn { get; private set; }
        [JsonProperty("E")] public int UsageManaCost { get; private set; }
        [JsonProperty("F")] public int UsedQuantity { get; private set; }
        [JsonProperty("G")] public Dictionary<ItemSkillUsageAttributes, object> Attributes { get; private set; }


        public BattleSkillUsageDTO(IItemSkill itemSkill)
        {
            FinalizeTurn = itemSkill.FinalizeTurnInUse;
            SkillOwnerId = itemSkill.Owner.UnitData.UniqueId;
            UniqueSkillId = itemSkill.UniqueId;
            ItemSkill = itemSkill.SkillData.ItemSkill;
            UsageManaCost = itemSkill.UsageManaCost;

            this.Attributes = new Dictionary<ItemSkillUsageAttributes, object>();

            foreach (var attribute in itemSkill.GetTempAttributes())
                Attributes.Add(attribute.Key, attribute.Value);
        }

        public BattleSkillUsageDTO(IItemConsumableSkill skill, int useCount) : this(skill)
        {
            UsedQuantity = useCount;
        }

        public T GetAttribute<T>(ItemSkillUsageAttributes attribute)
        {
            if (!Attributes.TryGetValue(attribute, out object value))
                return default;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
