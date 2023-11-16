using Newtonsoft.Json;
using System;

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

        private UserItemDTO AddItem(UserItemDTO item)
        {
            item.UserItemID = ++IdCounter;
            Items.Add(item);
            return item;
        }
        public UserItemDTO AddStackable(ItemDTO item, int quantity)
        {
            UserItemDTO inventoryItem = Items.Find(y => y.ItemID == item.Id);

            if (inventoryItem == null)
            {
                inventoryItem = new UserItemDTO
                {
                    ItemID = item.Id,
                    Quantity = quantity,
                    IsNew = true
                };

                AddItem(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += quantity;
            }

            return inventoryItem;
        }

        public UserItemDTO AddNonStackableItem(ItemDTO item, int level, float quality)
        {
            UserItemDTO inventoryItem = new UserItemDTO
            {
                ItemID = item.Id,
                Quantity = 1,
                Quality = quality,
                Level = level,
                IsNew = true,
            };

            return AddItem(inventoryItem);
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
        [JsonProperty("A")] public int EquippedUserItemId { get; set; }
        [JsonProperty("B")] public int UnequippedUserItemId { get; set;}
    }

}
