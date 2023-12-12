using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Services;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Models;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void CompleteCampaign(IBattleUser battleUser, long userId)
        {
            Task.Run(() =>
            {
                lock (battleUser.SocketUser)
                {
                    using ISocketMethodParameter smp = new SocketMethodParameter(battleUser.SocketUser, null);

                    TrackedUser userData = UserService.GetTrackedUser(smp, userId);

                    // WE GET THE CAMPAIGN DATA.
                    CampaignDTO campaign = userData.GetCampaign();

                    // WHEN THIS LEVEL COMPLETED FOR THE FIRST TIME.
                    bool firstTimeVictory = !campaign.IsLevelAlreadyCompleted(_levelData.Stage, _levelData.Level);

                    // WE COMPLETE THE CAMPAIGN FOR THE USER.
                    campaign.SaveLevelProgress(_levelData.Stage, _levelData.Level, isCompleted: true);
                    userData.UpdateCampaign(campaign);

                    // ALL THE REWARDS EARNED IN THE GAME.
                    List<IInventoryItemDTO> rewards = new List<IInventoryItemDTO>();
                    rewards.AddRange(battleUser.LootInventory.IItems);

                    // IF CAMPAIGN EARNED FIRST TIME.
                    if (firstTimeVictory)
                        rewards.AddRange(_levelData.IFirstCompletionRewards);

                    if (rewards.Count > 0)
                    {
                        // IF THIS IS THE FIRST TIME ITS COMPLETED.
                        InventoryDTO inventory = userData.GetInventory();

                        // WE ADD VICTORY REWARD ITEMS INTO INVENTORY. 
                        foreach (IInventoryItemDTO reward in rewards)
                        {
                            // WE MAKE SURE ITEM EXISTS.
                            IItemDTO itemData = ItemService.GetItem(reward.ItemID);
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

                        // WE REMOVE SPENT ITEMS.
                        foreach (IInventoryItemDTO equipment in battleUser.Equipments)
                        {
                            var itemData = ItemService.GetItem(equipment.ItemID);
                            switch (itemData.TypeId)
                            {
                                case Game.Enums.ItemTypes.Potion:
                                    {

                                    }
                                    break;
                            }
                        }

                        userData.UpdateInventory(inventory);
                    }

                    smp.UOW.SaveChanges();
                    smp.ExecuteOnSuccess();
                }
            });
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            GameOver = true;

            OnDisposed?.Invoke(this);
        }
    }
}
