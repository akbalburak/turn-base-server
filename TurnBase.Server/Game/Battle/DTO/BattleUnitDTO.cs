using Newtonsoft.Json;

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

        public BattleUnitDTO()
        {
            Skills = Array.Empty<BattleSkillDTO>();
        }
    }
}
