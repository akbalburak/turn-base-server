using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Item;
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
        private bool _isFinalizing;

        private void FinalizeBattle(bool isVictory)
        {
            if (_isFinalizing)
                return;

            _isFinalizing = true;

            // WE FINALIZE GAME FOR ALL USERS.
            foreach (IBattleUser user in _users)
                FinalizeBattleForUser(user, isVictory: isVictory);
        }

        private void FinalizeBattleForUser(IBattleUser battleUser, bool isVictory)
        {
            Task.Run(() =>
            {
                lock (battleUser.SocketUser)
                {
                    using ISocketMethodParameter smp = new SocketMethodParameter(battleUser.SocketUser, null);

                    TrackedUser userData = UserService.GetTrackedUser(smp, battleUser.SocketUser.User.Id);

                    List<IStoreableItemDTO> userRewards = new List<IStoreableItemDTO>();

                    // ALL THE REWARDS EARNED IN THE GAME.
                    userRewards.AddRange(battleUser.LootInventory.IItems);

                    // ONLY IF VICTORY WE WILL GIVE REWARDS.
                    if (isVictory)
                    {
                        // WE GET THE CAMPAIGN DATA.
                        CampaignDTO campaign = userData.GetCampaign();

                        // WHEN THIS LEVEL COMPLETED FOR THE FIRST TIME.
                        bool firstTimeVictory = !campaign.IsLevelAlreadyCompleted(_levelData.Stage, _levelData.Level);

                        // WE COMPLETE THE CAMPAIGN FOR THE USER.
                        campaign.SaveLevelProgress(_levelData.Stage, _levelData.Level, isCompleted: true);
                        userData.UpdateCampaign(campaign);

                        // IF CAMPAIGN EARNED FIRST TIME.
                        if (firstTimeVictory)
                            userRewards.AddRange(_levelData.IFirstCompletionRewards);
                    }

                    InventoryDTO inventory = userData.GetInventory();

                    // WE GAVE ALL USER EARNED REWARDS 
                    foreach (IStoreableItemDTO reward in userRewards)
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
                    foreach (IItemConsumableSkill consumableSkill in battleUser.GetConsumableSkills())
                    {
                        int inventoryItemId = consumableSkill.InventoryItem.InventoryItemID;
                        IEquipmentItemDTO inventoryItem = inventory.GetItem(inventoryItemId);
                        if (inventoryItem == null)
                            continue;

                        inventory.RemoveStackable(inventoryItem, consumableSkill.UsageCount);
                    }

                    userData.UpdateInventory(inventory);

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
