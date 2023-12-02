using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Item;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillUsageDTO
    {
        [JsonProperty("A")] public int UniqueSkillId { get; private set; }
        [JsonProperty("B")] public Enums.ItemSkills ItemSkill { get; private set; }
        [JsonProperty("C")] public int SkillOwnerId { get; private set; }
        [JsonProperty("D")] public List<BattleSkillUsageAttackItemDTO> Damages { get; private set; }
        [JsonProperty("E")] public bool FinalizeTurn { get; set; }
        [JsonProperty("F")] public bool DontStartCooldown { get; set; }
        [JsonProperty("G")] public int UsageManaCost { get; set; }
        [JsonProperty("H")] public int TargetNodeIndex { get; set; }
        [JsonProperty("I")] public int UsageStackCost { get; set; } 
        
        public BattleSkillUsageDTO(IItemSkill itemSkill)
        {
            FinalizeTurn = itemSkill.FinalizeTurnInUse;
            SkillOwnerId = itemSkill.Owner.UnitData.UniqueId;
            UniqueSkillId = itemSkill.UniqueId;
            ItemSkill = itemSkill.SkillData.ItemSkill;
            UsageManaCost = itemSkill.UsageManaCost;

            Damages = new List<BattleSkillUsageAttackItemDTO>();
        }

        public void AddTargetNode(int targetNodeIndex)
        {
            TargetNodeIndex = targetNodeIndex;
        }
        public void AddStackUsageCost(int stackUsageCost)
        {
            UsageStackCost = stackUsageCost;
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
