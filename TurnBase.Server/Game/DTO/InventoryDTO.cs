using Newtonsoft.Json;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Models
{
    public class InventoryDTO
    {
        [JsonProperty("A")] public int IdCounter { get; set; }
        [JsonProperty("B")] public List<UserItemDTO> Items { get; set; }
        
        private IChangeHandler _changeHandler;

        public InventoryDTO()
        {
            Items = new List<UserItemDTO>();
        }

        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            Items.ForEach(e => e.SetChangeHandler(_changeHandler));
        }

        public void AddStackable(IItemDTO item, int quantity)
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

            userItem.SetAsModified();
        }
        public void AddNonStackableItem(IItemDTO item, int level, float quality)
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

            userItem.SetAsModified();
        }

        public void RemoveStackable(IUserItemDTO userItem, int quantity)
        {
            if (userItem is not UserItemDTO userItemClass)
                return;

            userItemClass.Quality -= quantity;
            
            if (userItem.Quantity <= 0)
                Items.Remove(userItemClass);

            userItem.SetAsModified();
        }
        public void RemoveNonStackable(IUserItemDTO userItem)
        {
            if (userItem is not UserItemDTO userItemClass)
                return;

            Items.Remove(userItemClass);
            userItem.SetAsModified();
        }

        public IUserItemDTO GetItem(int userItemId)
        {
            return Items.FirstOrDefault(y => y.UserItemID == userItemId);
        }
        public IUserItemDTO[] GetEquippedItems()
        {
            return Items.Where(item => item.Equipped).ToArray();
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
