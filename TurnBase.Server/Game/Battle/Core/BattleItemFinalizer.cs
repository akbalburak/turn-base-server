using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void CompleteCampaign(ISocketUser user, long userId)
        {
            Task.Run(() =>
            {
                lock (user)
                {
                    using ISocketMethodParameter smp = new SocketMethodParameter(user, null);

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
                            IItemDTO itemData = ItemService.GetItem(reward.ItemId);
                            if (itemData == null)
                                continue;

                            // IF ITEM CAN STACK WE ADD AS STACKABLE.
                            if (itemData.CanStack)
                            {
                                inventory.AddStackable(
                                    item: itemData,
                                    quantity: reward.Quantity
                                );
                            }
                            else
                            {
                                inventory.AddNonStackableItem(
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
