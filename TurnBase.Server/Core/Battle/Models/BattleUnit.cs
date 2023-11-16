using Newtonsoft.Json;
using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Enums;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Controllers;
using TurnBase.Server.Core.Services;

namespace TurnBase.Server.Core.Battle.Models
{
    public abstract class BattleUnit
    {
        private static Random _random = new Random();

        public int TeamIndex { get; set; }
        public int UniqueId { get; private set; }

        public int Position { get; private set; }
        public int Health { get; private set; }
        public bool IsDeath { get; set; }

        public UnitStats Stats { get; set; }

        public List<BaseBattleSkill> Skills { get; set; }

        protected BattleUnit(int position, UnitStats stats)
        {
            Stats = stats;
            Position = position;
            Skills = new List<BaseBattleSkill>();

            Health = Stats.MaxHealth;
        }

        public void SetTeam(int teamIndex)
        {
            TeamIndex = teamIndex;
        }
        public void SetId(int id) { UniqueId = id; }
        public void ReduceHealth(int reduction)
        {
            Health -= reduction;

            if (Health <= 0)
                Kill();
        }
        public void Kill()
        {
            IsDeath = true;
        }

        public Action OnTurnStart;
        public void CallTurnStart()
        {
            OnTurnStart?.Invoke();
        }


        public int GetDamage(BattleUnit targetUnit)
        {
            bool isCritical = _random.NextDouble() <= Stats.CriticalChance;
            int damage = Stats.Damage;

            if (isCritical)
            {
                float bonusPercent = Stats.CriticalDamageBonus;
                damage += (int)Math.Round(damage * bonusPercent);
            }

            float targetArmor = targetUnit.Stats.PhysicalArmor;
            float reduction = 1 - targetArmor / (targetArmor + 100);
            return (int)Math.Round(damage * reduction);
        }

        public void AttackTo(BattleUnit defender, int damage)
        {
            defender.ReduceHealth(damage);
        }

        public void AddSkill(BaseBattleSkill skill)
        {
            Skills.Add(skill);
        }
        public void UseSkill(BattleSkillUseDTO useData)
        {
            BaseBattleSkill? skillToUse = Skills.Find(x => x.UniqueId == useData.UniqueSkillID);
            if (skillToUse == null)
                return;

            if (!skillToUse.IsSkillReadyToUse())
                return;

            skillToUse.UseSkill(useData);
        }
    }

    public record class UnitStats
    {
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamageBonus { get; set; }
        public int PhysicalArmor { get; set; }

        public void SetUser(TblUser user)
        {
            MaxHealth = ParameterService.GetIntValue(Parameters.BaseHealth);

            InventoryDTO inventory = user.GetInventory();

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
