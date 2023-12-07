using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleInventory : IBattleInventory
    {
        public IBattleUser Owner { get; }

        private int _inventoryItemId;

        private List<BattleInventoryItem> _inventory;

        public BattleInventory(IBattleUser owner)
        {
            _inventory = new List<BattleInventoryItem>();
            Owner = owner;
        }

        public void AddItem(IBattleDropItem item)
        {
            // WE FIND ITEM FOR 
            IItemDTO itemData = ItemService.GetItem(item.ItemId);
            if (itemData == null)
                return;

            if (itemData.CanStack)
            {
                BattleInventoryItem? invItem = _inventory.Find(x => x.ItemId == item.ItemId);
                if (invItem == null)
                {
                    _inventory.Add(new BattleInventoryItem(++_inventoryItemId, item));
                }
                else
                {
                    invItem.Quantity += item.Quantity;
                }
            }
            else
            {
                _inventory.Add(new BattleInventoryItem(++_inventoryItemId, item));
            }
        }

        private class BattleInventoryItem
        {
            public int InventoryItemId { get; set; }
            public int ItemId { get; set; }
            public int Quantity { get; set; }
            public float Quality { get; set; }
            public int Level { get; set; }
            public BattleInventoryItem(int inventoryItemId, IBattleDropItem item)
            {
                InventoryItemId = inventoryItemId;
                ItemId = item.ItemId;
                Quality = item.Quality;
                Quantity = item.Quantity;
                Level = item.Level;
            }
        }
    }

    
}
