using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Skills;

namespace TurnBase.Server.Core.Battle.Models
{
    public abstract class BattleUnit : IBattleUnit
    {
        private static Random _random = new Random();

        public Action OnUnitTurnStart { get; set; }

        public int UniqueId { get; private set; }

        public int TeamIndex { get; private set; }
        public int Position { get; private set; }

        public int Health { get; private set; }
        public bool IsDeath { get; private set; }

        public BattleUnitStats Stats { get; private set; }
        public List<ISkill> Skills { get; private set; }

        public IBattleItem Battle { get; private set; }

        protected BattleUnit(int position)
        {
            Skills = new List<ISkill>();
            Stats = new BattleUnitStats();
            Position = position;
        }

        public void SetTeam(int teamIndex)
        {
            this.TeamIndex = teamIndex;
        }
        public void SetId(int id)
        {
            this.UniqueId = id;
        }
        public void SetBattle(IBattleItem battleItem)
        {
            this.Battle = battleItem;
        }

        public void Kill()
        {
            IsDeath = true;
        }

        public int GetBaseDamage(IBattleUnit targetUnit)
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
        public void AttackToUnit(IBattleUnit defender, int damage)
        {
            defender.ReduceHealth(damage);
        }
        public void ReduceHealth(int reduction)
        {
            Health -= reduction;

            if (Health <= 0)
                Kill();
        }

        protected void AddSkill(ISkill skill)
        {
            Skills.Add(skill);
        }
        public void UseSkill(BattleSkillUseDTO useData)
        {
            ISkill skillToUse = Skills.Find(x => x.UniqueId == useData.UniqueSkillID);
            if (skillToUse == null)
                return;

            if (!skillToUse.IsSkillReadyToUse())
                return;

            skillToUse.UseSkill(useData);
        }

        public void CallUnitTurnStart()
        {
            OnUnitTurnStart?.Invoke();
        }

        public virtual void LoadSkills()
        {
        }
        public virtual void LoadStats(BattleUnitStats stats)
        {
            this.Stats = stats;

            Health = stats.MaxHealth;
        }
    }
}
