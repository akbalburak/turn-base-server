using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Controllers
{
    public static class ItemSkillController
    {
        public static SocketResponse GetItemSkills(ISocketMethodParameter smp)
        {
            return SocketResponse.GetSuccess(ItemSkillService.ItemSkills);
        }

        public static SocketResponse SwitchSkillSlot(ISocketMethodParameter smp)
        {
            ItemSkillSwitchDTO requestData = smp.GetRequestData<ItemSkillSwitchDTO>();

            // WE GET TRACKED USER.
            TrackedUser trackedUser = UserService.GetTrackedUser(smp, smp.SocketUser.User.Id);
            if (trackedUser == null)
                SocketResponse.GetError("User not found!");

            // LOADING USER INVENTORY.
            InventoryDTO inventory = trackedUser.GetInventory();

            // WE CHECK IF THE USER ITEM EXISTS.
            IInventoryItemDTO userItem = inventory.GetItem(requestData.UserItemId);
            if (userItem == null)
                return SocketResponse.GetError("User item not found!");

            // WE CHECK IF THE ITEM SELECTED SKILL SLOT IS VALID.
            IItemDTO itemData = ItemService.GetItem(userItem.ItemID);
            if (!itemData.Skills.Any(s => s.RowIndex == requestData.RowIndex &&
                s.ColIndex == requestData.ColIndex))
            {
                return SocketResponse.GetError("Invalid skill slot!");
            }

            // UPDATE RELATED USER ITEM WITH NEW SELECTION.
            userItem.ChangeActiveSkill(requestData.RowIndex, requestData.ColIndex);

            // WE UPDATE INVENTORY AND SAVE CHANGES.
            trackedUser.UpdateInventory(inventory);
            smp.UOW.SaveChanges();

            return SocketResponse.GetSuccess();
        }
    }
}
