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

        public BattleLoadAllDTO()
        {
            Units = Array.Empty<BattleNpcUnitDTO>();
            Players = Array.Empty<BattlePlayerDTO>();
            LootInventory = Array.Empty<BattleInventoryDTO>();
        }
    }
}
