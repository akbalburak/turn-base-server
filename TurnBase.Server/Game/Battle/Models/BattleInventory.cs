using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleInventory : IBattleInventory
    {
        public IBattleUser Owner { get; }
        public IInventoryItemDTO[] IItems => _inventory.ToArray();

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
            IItemDTO itemData = ItemService.GetItem(item.ItemID);
            if (itemData == null)
                return;

            if (itemData.CanStack)
            {
                BattleInventoryItem? invItem = _inventory.Find(x => x.ItemID == item.ItemID);
                if (invItem == null)
                    _inventory.Add(new BattleInventoryItem(++_inventoryItemId, item));
                else
                    invItem.AddQuantity(item.Quantity);
            }
            else
                _inventory.Add(new BattleInventoryItem(++_inventoryItemId, item));
        }
    }
    public class BattleInventoryItem : IInventoryItemDTO
    {
        public int InventoryItemID { get; }
        public int ItemID { get; }
        public int Quantity { get; private set; }
        public float Quality { get; private set; }
        public int Level { get; private set; }
        public BattleInventoryItem(int inventoryItemId, IBattleDropItem item)
        {
            InventoryItemID = inventoryItemId;
            ItemID = item.ItemID;
            Quality = item.Quality;
            Quantity = item.Quantity;
            Level = item.Level;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }

}
