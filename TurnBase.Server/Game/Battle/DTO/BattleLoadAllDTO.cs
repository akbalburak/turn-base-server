using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleLoadAllDTO
    {
        [JsonProperty("A")] public BattleNpcUnitDTO[] Units { get; set; }
        [JsonProperty("B")] public BattlePlayerDTO[] Players { get; set; }
        [JsonProperty("C")] public int LastDataId { get; set; }
        [JsonProperty("D")] public BattleTurnDTO TurnData { get; set; }
        [JsonProperty("E")] public BattleDropDTO[] Drops { get; set; }
        [JsonProperty("F")] public BattleInventoryDTO[] LootInventory { get; set; }
        [JsonProperty("G")] public bool IsInCombat { get; set; }
        [JsonProperty("H")] public int Stage { get; set; }
        [JsonProperty("I")] public int Level { get; set; }
        [JsonProperty("J")] public string ChatRoom { get; set; }
        public BattleLoadAllDTO()
        {
            Units = Array.Empty<BattleNpcUnitDTO>();
            Players = Array.Empty<BattlePlayerDTO>();
            LootInventory = Array.Empty<BattleInventoryDTO>();
        }
    }
}
