﻿using Newtonsoft.Json;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Models
{
    public class InventoryDTO : IInventoryDTO
    {
        [JsonProperty("A")] public int IdCounter { get; set; }
        [JsonProperty("B")] public List<InventoryItemDTO> Items { get; }

     
        private IChangeHandler _changeHandler;

        public InventoryDTO()
        {
            Items = new List<InventoryItemDTO>();
        }

        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            Items.ForEach(e => e.SetChangeHandler(_changeHandler));
        }

        public void AddStackable(IItemDTO item, int quantity)
        {
            InventoryItemDTO userItem = Items.Find(y => y.ItemID == item.Id);

            if (userItem == null)
            {
                userItem = new InventoryItemDTO
                {
                    ItemID = item.Id,
                    Quantity = quantity,
                    IsNew = true,
                    InventoryItemID = ++IdCounter,
                    SelectedSkills = new int[item.Skills.Length]
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
            InventoryItemDTO userItem = new InventoryItemDTO
            {
                InventoryItemID = ++IdCounter,
                ItemID = item.Id,
                Quantity = 1,
                Quality = quality,
                Level = level,
                IsNew = true,
                SelectedSkills = new int[item.Skills.Length]
            };

            userItem.SetChangeHandler(_changeHandler);

            Items.Add(userItem);

            userItem.SetAsModified();
        }

        public void RemoveStackable(IEquipmentItemDTO userItem, int quantity)
        {
            if (userItem is not InventoryItemDTO userItemClass)
                return;

            userItemClass.Quantity -= quantity;

            if (userItem.Quantity <= 0)
                Items.Remove(userItemClass);

            userItem.SetAsModified();
        }
        public void RemoveNonStackable(IEquipmentItemDTO userItem)
        {
            if (userItem is not InventoryItemDTO userItemClass)
                return;

            Items.Remove(userItemClass);
            userItem.SetAsModified();
        }

        public IEquipmentItemDTO GetItem(int userItemId)
        {
            return Items.FirstOrDefault(y => y.InventoryItemID == userItemId);
        }
        public IEquipmentItemDTO[] GetEquippedItems()
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
