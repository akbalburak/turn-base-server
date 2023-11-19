using TurnBase.Server.Core.Services;
using TurnBase.Server.Enums;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Battle.Models
{
    public class BattleUnitStats
    {
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamageBonus { get; set; }
        public int PhysicalArmor { get; set; }

        public BattleUnitStats()
        {
            
        }

        public BattleUnitStats(InventoryDTO inventory)
        {
            MaxHealth = ParameterService.GetIntValue(Parameters.BaseHealth);

            // WE LOOP ALL THE WORN ITEMS.
            foreach (UserItemDTO inventoryItem in inventory.Items)
            {
                if (!inventoryItem.Equipped)
                    continue;

                ItemDTO itemData = ItemService.GetItem(inventoryItem.ItemID);
                foreach (ItemPropertyDTO property in itemData.Properties)
                {
                    double value = property.GetValue(inventoryItem.Quality);
                    switch (property.PropertyId)
                    {
                        case ItemProperties.PhysicalDamage:
                            Damage += (int)value;
                            break;
                        case ItemProperties.MaxHealth:
                            MaxHealth += (int)value;
                            break;
                        case ItemProperties.TurnSpeed:
                            AttackSpeed += (float)value;
                            break;
                        case ItemProperties.CriticalChanceBonus:
                            CriticalChance += (float)value;
                            break;
                        case ItemProperties.CriticalDamageBonus:
                            CriticalDamageBonus += (float)value;
                            break;
                        case ItemProperties.PhysicalArmor:
                            PhysicalArmor += (int)value;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (AttackSpeed == 0)
                AttackSpeed = 1;
            else
                AttackSpeed = 1 / AttackSpeed;

            if (Damage == 0)
                Damage = 1;

        }
    }
}
