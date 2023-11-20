using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Skills;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillUsageDTO
    {
        [JsonProperty("A")] public int UniqueSkillId { get; private set; }
        [JsonProperty("B")] public BattleSkills Skill { get; private set; }
        [JsonProperty("C")] public int SkillOwnerId { get; private set; }
        [JsonProperty("D")] public List<BattleSkillUsageAttackItemDTO> Damages { get; private set; }
        [JsonProperty("E")] public bool FinalizeTurn { get; set; }

        public BattleSkillUsageDTO(IItemSkill skill)
        {
            FinalizeTurn = skill.FinalizeTurnInUse;
            SkillOwnerId = skill.Owner.UniqueId;
            UniqueSkillId = skill.UniqueId;
            Skill = skill.ItemSkill.SkillId;

            Damages = new List<BattleSkillUsageAttackItemDTO>();
        }

        public void AddToDamage(int defenderId, int damage)
        {
            Damages.Add(new BattleSkillUsageAttackItemDTO(
                defenderId,
                damage
            ));
        }
    }

    public class BattleSkillUsageAttackItemDTO
    {
        [JsonProperty("A")] public int DefenderId { get; set; }
        [JsonProperty("B")] public int Damage { get; set; }
        public BattleSkillUsageAttackItemDTO(int defenderId, int damage)
        {
            DefenderId = defenderId;
            Damage = damage;
        }

    }
}
