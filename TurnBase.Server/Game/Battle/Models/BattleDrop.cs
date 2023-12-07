using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleDrop : IBattleDrop
    {
        public Action<IBattleDrop> OnAllDropsClaimed { get; set; }
        public Action<IBattleDropItem> OnDropClaimed { get; set; }
        public IBattleUnit DropOwner { get; }
        public IBattleUnit KilledUnit { get; }
        public IBattleDropItem[] Drops { get; }

        public BattleDrop(IBattleUnit dropOwner, IBattleUnit killedUnit, IBattleDropItem[] drops)
        {
            DropOwner = dropOwner;
            KilledUnit = killedUnit;
            Drops = drops;
        }

        public void Collect(int index)
        {
            if (index < 0 || index >= Drops.Length)
                return;

            IBattleDropItem item = Drops[index];
            item.SetAsClaimed();

            OnClaimAnItem(item);
        }

        public void CollectAll()
        {
            foreach (BattleDropItem drop in Drops.ToArray())
            {
                if (drop.Claimed)
                    continue;

                drop.SetAsClaimed();
                OnClaimAnItem(drop);
            }
        }

        public void OnClaimAnItem(IBattleDropItem item)
        {

        }
    }

    public class BattleDropItem : IBattleDropItem
    {
        public int ItemId { get; }
        public int Level { get; }
        public float Quality { get; }
        public bool Claimed { get; private set; }

        public BattleDropItem(int itemId, int level, float quality)
        {
            ItemId = itemId;
            Level = level;
            Quality = quality;
        }

        public void SetAsClaimed()
        {
            Claimed = true;
        }
    }
}
