using System.Collections.Generic;

namespace ModuleDTOLayer
{
    public class InventoryDTO
    {
        public int IdCounter { get; private set; }
        public List<InventoryItemDTO> Items { get; private set; }

        public IInventoryItemDTO[] IItems => Items.ToArray();

        public InventoryDTO()
        {
            Items = new List<InventoryItemDTO>();
        }

        public InventoryItemDTO GetItem(int inventoryItemId)
        {
            return Items.Find(y => y.InventoryItemID == inventoryItemId);
        }
        public IInventoryItemDTO GetItemByItemId(int itemId)
        {
            return Items.Find(x => x.ItemID == itemId);
        }


        public void AddItem(bool canStack, IInventoryAddDTO addData)
        {
            IInventoryItemDTO existsItem = this.GetItemByItemId(addData.ItemID);

            // IF NON STACKABLE OR ITEM NOT EXISTS WE WILL ALWAYS CREATE ITEM.
            if (!canStack || existsItem == null)
            {
                InventoryItemDTO invItem = new InventoryItemDTO()
                {
                    InventoryItemID = ++IdCounter,
                    ItemID = addData.ItemID,
                    Level = addData.Level,
                    Quality = addData.Quality,
                    Quantity = addData.Quantity
                };

                Items.Add(invItem);
            }
            else
            {
                existsItem.AddQuantity(addData.Quantity);
            }
        }
    }
}
