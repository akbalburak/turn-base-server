using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Item;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillDTO
    {
        [JsonProperty("A")] public int UniqueID { get; private set; }
        [JsonProperty("B")] public Enums.ItemSkills Skill { get; private set; }

        [JsonProperty("C")] public int InitialCooldown { get; private set; }
        [JsonProperty("D")] public int CurrentCooldown { get; private set; }

        [JsonProperty("E")] public int InitialStackSize { get; private set; }
        [JsonProperty("F")] public int CurrentStackSize { get; private set; }

        [JsonProperty("G")] public float SkillQuality { get; private set; }
        [JsonProperty("H")] public int ConsumableCount { get; private set; }

        public BattleSkillDTO(IItemSkill skill)
        {
            this.Skill = skill.SkillData.ItemSkill;
            this.UniqueID = skill.UniqueId;
            this.InitialCooldown = skill.InitialCooldown;
            this.CurrentCooldown = skill.CurrentCooldown;
            this.SkillQuality = skill.SkillQuality;
        }
        public BattleSkillDTO(IItemStackableSkill skill) : this((IItemSkill)skill)
        {
            this.InitialStackSize = skill.InitialStackSize;
            this.CurrentStackSize = skill.CurrentStackSize;
        }

        public BattleSkillDTO(IItemConsumableSkill skill) : this((IItemSkill)skill)
        {
            ConsumableCount = skill.LeftUseCount;
        }
    }
}
