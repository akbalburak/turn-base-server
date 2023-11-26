using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public abstract class BattleUnitDTO
    {
        [JsonProperty("A")] public int UniqueId { get; set; }
        [JsonProperty("B")] public int Health { get; set; }
        [JsonProperty("C")] public int MaxHealth { get; set; }
        [JsonProperty("D")] public bool IsDead { get; set; }
        [JsonProperty("E")] public int Position { get; set; }
        [JsonProperty("F")] public int Damage { get; set; }
        [JsonProperty("G")] public float AttackSpeed { get; set; }
        [JsonProperty("H")] public BattleSkillDTO[] Skills { get; set; }
        [JsonProperty("I")] public int TeamIndex { get; set; }
        [JsonProperty("J")] public int Mana { get; set; }
        [JsonProperty("K")] public int MaxMana { get; set; }
        [JsonProperty("L")] public int NodeIndex { get; set; }

        public BattleUnitDTO()
        {
            Skills = Array.Empty<BattleSkillDTO>();
        }
    }
}
