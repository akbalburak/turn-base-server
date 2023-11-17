using Newtonsoft.Json;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Models
{
    public class InventoryDTO
    {

        [JsonProperty("A")] public int IdCounter { get; set; }
        [JsonProperty("B")] public List<UserItemDTO> Items { get; set; }

        public InventoryDTO()
        {
            Items = new List<UserItemDTO>();
        }

        public UserItemDTO GetItem(int userItemId)
        {
            return Items.FirstOrDefault(y => y.UserItemID == userItemId);
        }

        public UserItemDTO AddStackable(ItemDTO item, int quantity)
        {
            UserItemDTO userItem = Items.Find(y => y.ItemID == item.Id);

            if (userItem == null)
            {
                userItem = new UserItemDTO
                {
                    ItemID = item.Id,
                    Quantity = quantity,
                    IsNew = true,
                    UserItemID = ++IdCounter,
                };

                userItem.SetChangeHandler(_changeHandler);

                Items.Add(userItem);
            }
            else
            {
                userItem.Quantity += quantity;
            }

            userItem.SetAsChanged();

            return userItem;
        }
        public UserItemDTO AddNonStackableItem(ItemDTO item, int level, float quality)
        {
            UserItemDTO userItem = new UserItemDTO
            {
                UserItemID = ++IdCounter,
                ItemID = item.Id,
                Quantity = 1,
                Quality = quality,
                Level = level,
                IsNew = true,
            };

            userItem.SetChangeHandler(_changeHandler);

            Items.Add(userItem);

            userItem.SetAsChanged();

            return userItem;
        }

        public void RemoveStackable(UserItemDTO userItem, int quantity)
        {
            userItem.Quantity -= quantity;
            
            if (userItem.Quantity <= 0)
                Items.Remove(userItem);

            userItem.SetAsChanged();
        }
        public void RemoveNonStackable(UserItemDTO userItem)
        {
            Items.Remove(userItem);
            userItem.SetAsChanged();
        }

        private IChangeHandler _changeHandler;
        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            Items.ForEach(e => e.SetChangeHandler(_changeHandler));
        }

    }

    public class EquipItemRequestDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
    }

    public class EquipItemResponseDTO
    {
        [JsonProperty("A")] public int EquippedUserItemId { get; set; }
        [JsonProperty("B")] public int UnequippedUserItemId { get; set; }
    }

    public class UseItemRequestDTO
    {
        [JsonProperty("A")] public int UserItemId { get; set; }
    }
}
