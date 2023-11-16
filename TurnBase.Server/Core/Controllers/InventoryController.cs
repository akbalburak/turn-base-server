using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Enums;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Enums;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Controllers
{
    public static class InventoryController
    {
        public static InventoryDTO GetInventory(this TblUser user)
        {
            if (string.IsNullOrEmpty(user.Inventory))
                return new InventoryDTO();
            return user.Inventory.ToObject<InventoryDTO>();
        }

        public static void UpdateInventory(this TblUser user, InventoryDTO inventory)
        {
            user.Inventory = inventory.ToJson();
        }

        public static SocketResponse EquipItem(SocketMethodParameter smp)
        {
            EquipItemRequestDTO requestData = smp.GetRequestData<EquipItemRequestDTO>();

            // USER INFORMATION.
            TblUser user = smp.UOW.GetRepository<TblUser>()
                .Find(x => x.Id == smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            UserItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);
            int unequippedUserItemId = 0;
            if (inventoryItem != null)
            {
                // WE MAKE SURE THE SAME TYPE ITEM NOT WORN.
                ItemDTO itemData = ItemService.GetItem(inventoryItem.ItemID);

                // WE MAKE SURE THE ITEM IS VALID TYPE.
                if (itemData.Action != DTOLayer.Enums.ItemActions.Equipable)
                    return SocketResponse.GetError("Invalid Item To Wear!");

                // WE MAKE SURE THE SAME TYPE ITEM DIDN'T NOT WORN.
                List<UserItemDTO> equippedItems = inventory.Items.FindAll(y => y.Equipped);
                equippedItems.ForEach(e =>
                {
                    ItemDTO eItemData = ItemService.GetItem(e.ItemID);
                    if (eItemData.TypeId == itemData.TypeId)
                    {
                        e.Equipped = false;
                        unequippedUserItemId = e.UserItemID;
                    }
                });

                inventoryItem.Equipped = true;
            }

            user.UpdateInventory(inventory);

            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess(new EquipItemResponseDTO
            {
                EquippedUserItemId = requestData.UserItemId,
                UnequippedUserItemId = unequippedUserItemId
            });
        }

        public static SocketResponse UnequipItem(SocketMethodParameter smp)
        {
            EquipItemRequestDTO requestData = smp.GetRequestData<EquipItemRequestDTO>();

            // USER INFORMATION.
            TblUser user = smp.UOW.GetRepository<TblUser>()
                .Find(x => x.Id == smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            UserItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);
            if (inventoryItem != null)
                inventoryItem.Equipped = false;

            user.UpdateInventory(inventory);

            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess(new EquipItemResponseDTO
            {
                UnequippedUserItemId = requestData.UserItemId
            });
        }

        public static SocketResponse UseAnItem(SocketMethodParameter smp)
        {
            UseItemRequestDTO requestData = smp.GetRequestData<UseItemRequestDTO>();

            // USER INFORMATION.
            TblUser user = smp.UOW.GetRepository<TblUser>()
                .Find(x => x.Id == smp.SocketUser.User.Id);

            // LOAD PLAYER INVENTORY.
            InventoryDTO inventory = user.GetInventory();

            // WE GET THE INVENTORY ITEM.
            UserItemDTO inventoryItem = inventory.GetItem(requestData.UserItemId);

            // IF ITEM DOES NOT EXISTS.
            if (inventoryItem == null || inventoryItem.Quantity <= 0)
                return SocketResponse.GetError("Not Enough Quantity!");

            // WE GET ITEM META DATA.
            ItemDTO itemData = ItemService.GetItem(inventoryItem.ItemID);

            // WE MAKE SURE THE ITEM IS USABLE.
            if (itemData.Action != ItemActions.Usable)
                return SocketResponse.GetError("Invalid Item To Wear!");

            // CHANGES TO TELL PLAYER.
            InventoryModifiedDTO inventoryChanges = new InventoryModifiedDTO();
            inventoryChanges.Items.Add(new InventoryModifiedItemDTO(
                userItemId: inventoryItem.UserItemID,
                itemId: inventoryItem.ItemID,
                quantity: 1,
                isAdd: false
            ));

            // WE REMOVE ONE ITEM.
            if (itemData.CanStack)
                inventory.RemoveStackable(inventoryItem, 1);
            else
                inventory.RemoveNonStackable(inventoryItem);

            // WE GAVE THE REWARDS DEPENDS ON ITEM CONTENT.
            foreach (ItemContentDTO content in itemData.Contents)
            {
                switch (content.ContentId)
                {
                    case ItemContents.Coins:
                        user.Gold += (int)content.Value;
                        break;
                }
            }

            user.UpdateInventory(inventory);

            smp.UOW.SaveChanges();

            // WE TELL USER WE GAVE YOU SOME REWARDS.
            smp.SocketUser.AddToUnExpectedAfterSendIt(SocketResponse.GetSuccess(
                ActionTypes.InventoryModified,
                inventoryChanges
            ));

            return SocketResponse.GetSuccess();
        }

    }
}
