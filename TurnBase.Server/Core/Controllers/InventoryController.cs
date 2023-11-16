using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Core.Services;
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
                switch (itemData.TypeId)
                {
                    case DTOLayer.Enums.ItemTypes.Potion:
                    case DTOLayer.Enums.ItemTypes.Food:
                    case DTOLayer.Enums.ItemTypes.MoneyBag:
                        return SocketResponse.GetError("Invalid Item To Wear!");
                }

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
    }
}
