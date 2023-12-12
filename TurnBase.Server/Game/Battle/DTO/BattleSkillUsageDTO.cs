using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Item;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleSkillUsageDTO
    {
        [JsonProperty("A")] public int UniqueSkillId { get; private set; }
        [JsonProperty("B")] public Enums.ItemSkills ItemSkill { get; private set; }
        [JsonProperty("C")] public int SkillOwnerId { get; private set; }
        [JsonProperty("D")] public List<BattleSkillUsageDamageDTO> Damages { get; private set; }
        [JsonProperty("E")] public bool FinalizeTurn { get; private set; }
        [JsonProperty("F")] public bool DontStartCooldown { get; set; }
        [JsonProperty("G")] public int UsageManaCost { get; private set; }
        [JsonProperty("H")] public int TargetNodeIndex { get; private set; }
        [JsonProperty("I")] public int UsageStackCost { get; private set; }
        [JsonProperty("J")] public List<BattleSkillUsageRecoveryDTO> Recoveries { get; private set; }
        [JsonProperty("K")] public int UsedQuantity { get; private set; }

        public BattleSkillUsageDTO(IItemSkill itemSkill)
        {
            FinalizeTurn = itemSkill.FinalizeTurnInUse;
            SkillOwnerId = itemSkill.Owner.UnitData.UniqueId;
            UniqueSkillId = itemSkill.UniqueId;
            ItemSkill = itemSkill.SkillData.ItemSkill;
            UsageManaCost = itemSkill.UsageManaCost;

            Damages = new List<BattleSkillUsageDamageDTO>();
            Recoveries = new List<BattleSkillUsageRecoveryDTO>();
        }

        public BattleSkillUsageDTO(IItemConsumableSkill skill, int useCount) : this(skill)
        {
            UsedQuantity = useCount;
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
            Damages.Add(new BattleSkillUsageDamageDTO(
                defenderId,
                damage
            ));
        }
        public void AddRecovery(int targetUnitId, int recovery)
        {
            Recoveries.Add(new BattleSkillUsageRecoveryDTO(
                targetUnitId,
                recovery
            ));
        }
    }

    public class BattleSkillUsageDamageDTO
    {
        [JsonProperty("A")] public int DefenderId { get; set; }
        [JsonProperty("B")] public int Damage { get; set; }
        public BattleSkillUsageDamageDTO(int defenderId, int damage)
        {
            DefenderId = defenderId;
            Damage = damage;
        }
    }

    public class BattleSkillUsageRecoveryDTO
    {
        [JsonProperty("A")] public int TargetUnitId { get; set; }
        [JsonProperty("B")] public int RecoveryValue { get; set; }
        public BattleSkillUsageRecoveryDTO(int targetUnitId, int recoveryValue)
        {
            TargetUnitId = targetUnitId;
            RecoveryValue = recoveryValue;
        }
    }
}
