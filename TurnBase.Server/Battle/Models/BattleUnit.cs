using Newtonsoft.Json;
using TurnBase.DBLayer.Models;
using TurnBase.DTOLayer.Enums;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Battle.Core.Skills;
using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Controllers;
using TurnBase.Server.Services.Item;
using TurnBase.Server.Services.Parameter;

namespace TurnBase.Server.Battle.Models
{
    public abstract class BattleUnit
    {
        private static Random _random = new Random();

        public int TeamIndex { get; set; }
        public int Id { get; private set; }

        public int Position { get; private set; }
        public int Health { get; private set; }
        public bool IsDeath { get; set; }

        public UnitStats Stats { get; set; }

        public List<BaseBattleSkill> Skills { get; set; }

        protected BattleUnit(int position, UnitStats stats)
        {
            this.Stats = stats;
            this.Position = position;
            this.Skills = new List<BaseBattleSkill>();

            this.Health = this.Stats.MaxHealth;
        }

        public void SetTeam(int teamIndex)
        {
            this.TeamIndex = teamIndex;
        }
        public void SetId(int id) { Id = id; }
        public void ReduceHealth(int reduction)
        {
            this.Health -= reduction;

            if (this.Health <= 0)
                Kill();
        }
        public void Kill()
        {
            this.IsDeath = true;
        }

        public Action OnTurnStart;
        public void CallTurnStart()
        {
            OnTurnStart?.Invoke();
        }


        public int GetDamage(BattleUnit targetUnit)
        {
            bool isCritical = _random.NextDouble() <= this.Stats.CriticalChance;
            int damage = this.Stats.Damage;

            if (isCritical)
            {
                float bonusPercent = this.Stats.CriticalDamageBonus;
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
            this.Skills.Add(skill);
        }
        public void UseSkill(BattleSkillUseDTO useData)
        {
            BaseBattleSkill? skillToUse = this.Skills.Find(x => x.UniqueId == useData.UniqueSkillID);
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
            this.MaxHealth = ParameterService.GetIntValue(Parameters.BaseHealth);

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
                            this.Damage += (int)value;
                            break;
                        case ItemProperties.MaxHealth:
                            this.MaxHealth += (int)value;
                            break;
                        case ItemProperties.TurnSpeed:
                            this.AttackSpeed += (float)value;
                            break;
                        case ItemProperties.CriticalChanceBonus:
                            this.CriticalChance += (float)value;
                            break;
                        case ItemProperties.CriticalDamageBonus:
                            this.CriticalDamageBonus += (float)value;
                            break;
                        case ItemProperties.PhysicalArmor:
                            this.PhysicalArmor += (int)value;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (this.AttackSpeed == 0)
                this.AttackSpeed = 1;
            else
                this.AttackSpeed = 1 / this.AttackSpeed;

            if (this.Damage == 0)
                this.Damage = 1;

        }
    }

}
