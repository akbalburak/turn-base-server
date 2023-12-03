using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public abstract class BattleUnitDTO
    {
        [JsonProperty("A")] public int UniqueId { get; set; }
        [JsonProperty("B")] public int Health { get; set; }
        [JsonProperty("C")] public int MaxHealth { get; set; }
        [JsonProperty("D")] public bool IsDead { get; set; }
        [JsonProperty("E")] public int Damage { get; set; }
        [JsonProperty("F")] public float AttackSpeed { get; set; }
        [JsonProperty("G")] public BattleSkillDTO[] Skills { get; set; }
        [JsonProperty("H")] public int TeamIndex { get; set; }
        [JsonProperty("I")] public int Mana { get; set; }
        [JsonProperty("J")] public int MaxMana { get; set; }
        [JsonProperty("K")] public int NodeIndex { get; set; }

        public BattleUnitDTO(IBattleItem battleItem, IBattleUnit battleUnit)
        {
            UniqueId = battleUnit.UnitData.UniqueId;
            AttackSpeed = battleUnit.Stats.AttackSpeed;
            Health = battleUnit.Health;
            Mana = battleUnit.Mana;
            Damage = battleUnit.Stats.Damage;

            MaxHealth = battleUnit.Stats.MaxHealth;
            MaxMana = battleUnit.Stats.MaxMana;
            IsDead = battleUnit.IsDeath;
            TeamIndex = battleUnit.UnitData.TeamIndex;
            NodeIndex = battleItem.GetNodeIndex(battleUnit.CurrentNode);
            Skills = battleUnit.Skills.Select(v => v.GetSkillDataDTO()).ToArray();
        }
    }
}
