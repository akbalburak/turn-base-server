using Newtonsoft.Json;

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

    public class InventoryModifiedItemDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
        [JsonProperty("B")] public int ItemId { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public int Level { get; set; }
        [JsonProperty("E")] public float Quality { get; set; }

        public InventoryModifiedItemDTO(int userItemId, int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
            UserItemId = userItemId;
        }

        public InventoryModifiedItemDTO(int userItemId, int itemId, int level, float quality)
        {
            ItemId = itemId;
            Quantity = 1;
            Level = level;
            Quality = quality;
            UserItemId = userItemId;
        }
    }
}
