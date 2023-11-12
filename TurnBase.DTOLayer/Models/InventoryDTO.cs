using Newtonsoft.Json;

namespace TurnBase.DTOLayer.Models
{
    public class InventoryDTO
    {
        [JsonProperty("A")] public int IdCounter { get; set; }
        [JsonProperty("B")] public List<UserItemDTO> Items { get; set; }
        public InventoryDTO()
        {
            Items = new List<UserItemDTO>();
        }

        public void AddItem(UserItemDTO item)
        {
            item.UserItemID = ++IdCounter;
            Items.Add(item);
        }

        public UserItemDTO GetItem(int userItemId)
        {
            return Items.FirstOrDefault(y => y.UserItemID == userItemId);
        }
    }

    public class EquipItemRequestDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
    }

    public class EquipItemResponseDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
    }

}
