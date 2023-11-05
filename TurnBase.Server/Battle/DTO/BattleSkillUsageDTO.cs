using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleSkillUsageDTO
    {
        [JsonProperty("A")] public int UniqueSkillId { get; private set; }
        [JsonProperty("B")] public BattleSkills Skill { get; private set; }
        [JsonProperty("C")] public int SkillOwnerId { get; private set; }
        [JsonProperty("D")] public List<BattleSkillUsageAttackItemDTO> Damages { get; private set; }
        public BattleSkillUsageDTO(int skillOwnerId,int uniqueSkillId, BattleSkills skill)
        {
            this.SkillOwnerId = skillOwnerId;
            this.UniqueSkillId = uniqueSkillId;
            this.Skill = skill;
            this.Damages = new List<BattleSkillUsageAttackItemDTO>();
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
            this.DefenderId = defenderId;
            this.Damage = damage;
        }

    }
}
