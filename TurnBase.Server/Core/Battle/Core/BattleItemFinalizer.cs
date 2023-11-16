using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.DTOLayer.Enums;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Enums;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Core
{
    public partial class BattleItem
    {
        public void CompleteCampaign(SocketUser user, long userId)
        {
            Task.Run(() =>
            {
                lock (user)
                {
                    using IUnitOfWork uow = new UnitOfWork();

                    TblUser userData = uow.GetRepository<TblUser>()
                        .Find(x => x.Id == userId);

                    // WE GET THE CAMPAIGN DATA.
                    CampaignDTO campaign = userData.GetCampaign();

                    // WHEN THIS LEVEL COMPLETED FOR THE FIRST TIME.
                    bool firstTimeVictory = !campaign.IsDifficulityCompleted(_levelData.Stage, _levelData.Level, _difficulity);

                    // WE COMPLETE THE CAMPAIGN FOR THE USER.
                    campaign.AddStageProgress(_levelData.Stage, _levelData.Level, _difficulity);
                    userData.UpdateCampaign(campaign);

                    // IF THIS IS THE FIRST TIME ITS COMPLETED.
                    if (firstTimeVictory)
                    {
                        // WE NEED TO SEND ADDED ITEMS TO USER.
                        InventoryModifiedDTO inventoryChanges = new InventoryModifiedDTO();

                        // WE ADD VICTORY REWARD ITEMS INTO INVENTORY. 
                        InventoryDTO inventory = userData.GetInventory();
                        foreach (BattleRewardItemData reward in _difficulityData.FirstCompletionRewards)
                        {
                            // WE MAKE SURE ITEM EXISTS.
                            ItemDTO itemData = ItemService.GetItem(reward.ItemId);
                            if (itemData == null)
                                continue;

                            // IF ITEM CAN STACK WE ADD AS STACKABLE.
                            if (itemData.CanStack)
                            {
                                UserItemDTO addedItem = inventory.AddStackable(
                                    item: itemData,
                                    quantity: reward.Quantity
                                );

                                inventoryChanges.Items.Add(new InventoryModifiedItemDTO(
                                    userItemId: addedItem.UserItemID,
                                    itemId: reward.ItemId,
                                    quantity: reward.Quantity,
                                    isAdd: true
                                ));
                            }
                            else
                            {
                                UserItemDTO addedItem = inventory.AddNonStackableItem(
                                    item: itemData,
                                    level: reward.Level,
                                    quality: reward.Quality
                                );

                                inventoryChanges.Items.Add(new InventoryModifiedItemDTO(
                                   userItemId: addedItem.UserItemID,
                                   itemId: reward.ItemId,
                                   level: reward.Level,
                                   quality: reward.Quality,
                                   isAdd: true
                               ));
                            }

                        }
                        userData.UpdateInventory(inventory);

                        uow.SaveChanges();

                        // WE TELL USER WE GAVE YOU SOME REWARDS.
                        user.AddToUnExpectedAfterSendIt(SocketResponse.GetSuccess(
                            ActionTypes.InventoryModified,
                            inventoryChanges
                        ));
                    }
                    else
                    {
                        uow.SaveChanges();
                    }
                }
            });
        }
    }
}
