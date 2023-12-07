using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Game.Battle.Models;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {

        private void OnUnitDie(IBattleUnit unit)
        {
            // WE MAKE SURE THERE IS A KILLER.
            IBattleUnit killedBy = unit.KilledBy;
            if (killedBy == null)
                return;

            // IF NPC KILLED PLAYER NO LONGER NEED TO GO DOWN.
            if (killedBy is IBattleNpcUnit)
                return;

            // WE GET ALL THE TEAM MEMBERS OF THE KILLER.
            IBattleUser[] killerTeam = _users
                .Where(x => x.UnitData.TeamIndex == killedBy.UnitData.TeamIndex)
                .ToArray();

            // ALL TEAM IS GOING TO TAKE REWARD IF THEY LUCKY ENOUGH.
            List<IBattleDropItem> dropItems = new List<IBattleDropItem>();
            foreach (IBattleUser user in killerTeam)
            {
                dropItems.Clear();

                // WE LOOP ALL THE POSSIBLE DROPS.
                int dropItemId = 0;
                foreach (IMapDataEnemyDropJson drop in unit.UnitData.IDrops)
                {
                    double chance = GetRandomValue;

                    // IF NOT LUCKY ENOUGH JUST SKIP IT.
                    if (chance > drop.DropChance)
                        continue;

                    // WEGET A RANDOM QUANTITY.
                    int quantity = _randomizer.Next(drop.Quantity.X, drop.Quantity.Y);
                    if (quantity == 0)
                        continue;

                    dropItems.Add(new BattleDropItem(++dropItemId, drop.ItemId, drop.Level, drop.Quality, quantity));
                }

                // WHEN THERE IS A LOOT WE CREATE A GROUP FOR IT.
                if (dropItems.Count > 0)
                {
                    BattleDrop dropGroup = new(this, user, unit, dropItems.ToArray());
                    dropGroup.OnAllDropsClaimed += OnAllDropsClaimed;

                    lock (_drops)
                        _drops.Add(dropGroup);

                    SendToUser(user, Enums.BattleActions.YouHaveDrop, new BattleDropDTO(dropGroup));
                }
            }
        }

        private void OnAllDropsClaimed(IBattleDrop drop)
        {
            lock (_drops)
                _drops.Remove(drop);
        }

    }
}
