using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Enums;
using TurnBase.Server.Models;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Trackables;

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
                    using SocketMethodParameter smp = new SocketMethodParameter(user, null);

                    TrackedUser userData = UserService.GetTrackedUser(smp, userId);

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
                            }
                            else
                            {
                                UserItemDTO addedItem = inventory.AddNonStackableItem(
                                    item: itemData,
                                    level: reward.Level,
                                    quality: reward.Quality
                                );
                            }

                        }
                        userData.UpdateInventory(inventory);

                    }

                    smp.UOW.SaveChanges();
                    smp.ExecuteOnSuccess();
                }
            });
        }
    }
}
