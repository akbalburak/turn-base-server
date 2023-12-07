using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleDrop : IBattleDrop
    {
        public Action<IBattleDrop> OnAllDropsClaimed { get; set; }
        public Action<IBattleDropItem> OnDropClaimed { get; set; }
        public IBattleItem Battle { get; set; }
        public IBattleUser DropOwner { get; }
        public IBattleUnit KilledUnit { get; }
        public IBattleDropItem[] Drops { get; }

        public BattleDrop(IBattleItem battle,
                          IBattleUser dropOwner,
                          IBattleUnit killedUnit,
                          IBattleDropItem[] drops)
        {
            Battle = battle;
            DropOwner = dropOwner;
            KilledUnit = killedUnit;
            Drops = drops;
        }

        public void Claim(int dropItemId)
        {
            if (dropItemId == -1)
                ClaimAll();
            else
                ClaimOne(dropItemId);
        }

        private void ClaimOne(int dropItemId)
        {
            // WE SEARCH FOR THE DROP ITEM IN LOOT.
            IBattleDropItem item = Drops.FirstOrDefault(x => x.DropItemId == dropItemId);
            if (item.Claimed)
                return;

            // AFTER FOUND IT WE SET AS CLAIMED.
            item.SetAsClaimed();

            OnClaimItem(new List<IBattleDropItem> { item });
        }

        private void ClaimAll()
        {
            List<IBattleDropItem> claims = new List<IBattleDropItem>();

            foreach (IBattleDropItem drop in Drops)
            {
                if (drop.Claimed)
                    continue;

                drop.SetAsClaimed();

                claims.Add(drop);
            }

            OnClaimItem(claims);
        }

        public void OnClaimItem(IEnumerable<IBattleDropItem> items)
        {
            // WE ADD ALL ITEMS INTO INVENTORY.
            foreach (IBattleDropItem item in items)
                DropOwner.LootInventory.AddItem(item);

            // WE SEND DROPS TO OWNER.
            Battle.SendToUser(DropOwner,
                Enums.BattleActions.ClaimADrop,
                new BattleDropClaimResponseDTO(KilledUnit.UnitData.UniqueId,
                    items.Select(x => x.DropItemId).ToArray()
                )
            );
        }
    }

    public class BattleDropItem : IBattleDropItem
    {
        public int DropItemId { get; }
        public int ItemId { get; }
        public int Level { get; }
        public float Quality { get; }
        public int Quantity { get; }

        public bool Claimed { get; private set; }

        public BattleDropItem(int dropItemId,
                              int itemId,
                              int level,
                              float quality,
                              int quantity)
        {
            DropItemId = dropItemId;
            ItemId = itemId;
            Level = level;
            Quality = quality;
            Quantity = quantity;
        }

        public void SetAsClaimed()
        {
            Claimed = true;
        }
    }
}
