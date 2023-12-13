using Newtonsoft.Json;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleInventoryDTO : IStoreableItemDTO
    {
        [JsonProperty("A")] public int ItemID { get; set; }
        [JsonProperty("B")] public float Quality { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public int Level { get; set; }

        public BattleInventoryDTO(IStoreableItemDTO item)
        {
            ItemID = item.ItemID;
            Quantity = item.Quantity;
            Quality = item.Quality;
            Level = item.Level;
        }
    }
}
