using Newtonsoft.Json;
using TurnBase.DTOLayer.Interfaces;

namespace TurnBase.DTOLayer.Models
{
    public class InventoryModifiedDTO
    {
        [JsonProperty("A")] public List<InventoryModifiedItemDTO> Items { get; }
        public InventoryModifiedDTO()
        {
            Items = new List<InventoryModifiedItemDTO>();
        }
    }

    public class InventoryModifiedItemDTO : IChangeItem
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
        [JsonProperty("B")] public int ItemId { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public int Level { get; set; }
        [JsonProperty("E")] public float Quality { get; set; }
        [JsonProperty("F")] public bool IsAdd { get; set; }
        public InventoryModifiedItemDTO(int userItemId, int itemId, int quantity, bool isAdd)
        {
            ItemId = itemId;
            Quantity = quantity;
            UserItemId = userItemId;
            this.IsAdd = isAdd;
        }

        public InventoryModifiedItemDTO(int userItemId, int itemId, int level, float quality, bool isAdd)
        {
            ItemId = itemId;
            Quantity = 1;
            Level = level;
            Quality = quality;
            UserItemId = userItemId;
            this.IsAdd = isAdd;
        }
    }
}
