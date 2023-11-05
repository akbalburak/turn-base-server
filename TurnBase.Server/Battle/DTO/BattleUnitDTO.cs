using Newtonsoft.Json;

namespace TurnBase.Server.Battle.DTO
{
    public abstract class BattleUnitDTO
    {
        [JsonProperty("A")] public int UniqueId { get; set; }
        [JsonProperty("B")] public int Health { get; set; }
        [JsonProperty("C")] public int MaxHealth { get; set; }
        [JsonProperty("D")] public bool IsDead { get; set; }
        [JsonProperty("E")] public int Position { get; set; }
        [JsonProperty("F")] public int MinDamage { get; set; }
        [JsonProperty("G")] public int MaxDamage { get; set; }
        [JsonProperty("H")] public float AttackSpeed { get; set; }
    }
}
