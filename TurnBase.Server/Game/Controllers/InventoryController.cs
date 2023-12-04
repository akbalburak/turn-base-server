using TurnBase.Server.Game.Services;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Controllers
{
    public static class InventoryController
    {
        public static SocketResponse EquipItem(ISocketMethodParameter smp)
        {
            EquipItemRequestDTO requestData = smp.GetRequestData<EquipItemRequestDTO>();

            // USER INFORMATION.
            TrackedUser user = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            IInventoryItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);
            if (inventoryItem != null)
            {
                // WE MAKE SURE THE SAME TYPE ITEM NOT WORN.
                IItemDTO itemData = ItemService.GetItem(inventoryItem.ItemID);

                // WE MAKE SURE THE ITEM IS VALID TYPE.
                if (itemData.Action != ItemActions.Equipable)
                    return SocketResponse.GetError("Invalid Item To Wear!");

                // WE MAKE SURE THE SAME TYPE ITEM DIDN'T NOT WORN.
                foreach (IInventoryItemDTO equippedItem in inventory.GetEquippedItems())
                {
                    IItemDTO eItemData = ItemService.GetItem(equippedItem.ItemID);
                    if (eItemData.TypeId != itemData.TypeId)
                        continue;

                    equippedItem.UpdateEquipState(false);
                }

                inventoryItem.UpdateEquipState(true);
            }

            // SAVE CHANGES ON DB SIDE TOO.
            user.UpdateInventory(inventory);
            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess();
        }

        public static SocketResponse UnequipItem(ISocketMethodParameter smp)
        {
            EquipItemRequestDTO requestData = smp.GetRequestData<EquipItemRequestDTO>();

            // USER INFORMATION.
            TrackedUser user = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            IInventoryItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);
            if (inventoryItem == null)
                return SocketResponse.GetError("Inventory item not found!");

            // WE CHANGE EQUIP STATE.
            inventoryItem.UpdateEquipState(false);

            // SAVE CHANGES IN DB SIDE TOO.
            user.UpdateInventory(inventory);
            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess();
        }

        public static SocketResponse UseAnItem(ISocketMethodParameter smp)
        {
            UseItemRequestDTO requestData = smp.GetRequestData<UseItemRequestDTO>();

            // USER INFORMATION.
            TrackedUser user = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            IInventoryItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);

            // IF ITEM DOES NOT EXISTS.
            if (inventoryItem == null || inventoryItem.Quantity <= 0)
                return SocketResponse.GetError("Not Enough Quantity!");

            // WE GET ITEM META DATA.
            IItemDTO itemData = ItemService.GetItem(inventoryItem.ItemID);

            // WE MAKE SURE THE ITEM IS USABLE.
            if (itemData.Action != ItemActions.Usable)
                return SocketResponse.GetError("Invalid Item To Wear!");

            // WE REMOVE ONE ITEM.
            if (itemData.CanStack)
                inventory.RemoveStackable(inventoryItem, 1);
            else
                inventory.RemoveNonStackable(inventoryItem);

            // WE GAVE THE REWARDS DEPENDS ON ITEM CONTENT.
            foreach (ItemContentMappingDTO content in itemData.Contents)
            {
                switch (content.ContentId)
                {
                    case ItemContents.Coins:
                        user.AddGolds((int)content.Value);
                        break;
                }
            }

            user.UpdateInventory(inventory);
            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess();
        }

    }
}
