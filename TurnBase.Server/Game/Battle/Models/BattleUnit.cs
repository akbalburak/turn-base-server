using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Models
{
    public abstract class BattleUnit : IBattleUnit, IAStarUnit
    {
        public Action<IBattleUnit> OnUnitTurnStart { get; set; }
        public Action<IBattleUnit> OnUnitDie { get; set; }

        public int UniqueId { get; private set; }

        public int TeamIndex { get; private set; }
        public IAStarNode CurrentNode { get; private set; }

        public int Health { get; private set; }
        public int Mana { get; private set; }
        public bool IsDeath { get; private set; }

        public BattleUnitStats Stats { get; private set; }

        public List<IItemSkill> Skills { get; private set; }
        public List<IItemSkillEffect> Effects { get; private set; }

        public IBattleItem Battle { get; private set; }

        protected BattleUnit()
        {
            Skills = new List<IItemSkill>();
            Effects = new List<IItemSkillEffect>();
            Stats = new BattleUnitStats();
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


        public int GetBaseDamage(IBattleUnit targetUnit)
        {
            bool isCritical = Battle.GetRandomValue <= Stats.CriticalChance;
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

        public void AddSkill(IItemSkill skill)
        {
            Skills.Add(skill);
        }
        public void UseSkill(BattleSkillUseDTO useData)
        {
            IItemSkill skillToUse = Skills.Find(x => x.UniqueId == useData.UniqueSkillID);
            if (skillToUse == null)
                return;

            if (!skillToUse.IsSkillReadyToUse())
                return;

            skillToUse.UseSkill(useData);
        }

        public void CallUnitTurnStart()
        {
            OnUnitTurnStart?.Invoke(this);
        }

        public void AddEffect(IItemSkillEffect effect)
        {
            if (effect == null)
                return;

            if (IsDeath)
                return;

            effect.OnEffectCompleted += OnEffectCompleted;
            Effects.Add(effect);
        }
        private void OnEffectCompleted(IItemSkillEffect effect)
        {
            effect.OnEffectCompleted -= OnEffectCompleted;
            Effects.Remove(effect);
        }

        public bool IsManaEnough(int manaCost)
        {
            return Mana >= manaCost;
        }
        public void ReduceMana(int usageManaCost)
        {
            Mana = usageManaCost;

            if (Mana < 0)
                Mana = 0;
        }

        public void Kill()
        {
            if (IsDeath)
                return;

            ChangeNode(null);
            IsDeath = true;
            OnUnitDie?.Invoke(this);
        }

        public virtual void LoadStats(BattleUnitStats stats)
        {
            this.Stats = stats;

            Health = stats.MaxHealth;
            Mana = stats.MaxMana;
        }
        public virtual void LoadSkills()
        {

        }

        public virtual void ChangeNode(IAStarNode node)
        {
            this.CurrentNode?.SetOwner(null);
            this.CurrentNode = node;
            this.CurrentNode?.SetOwner(this);
        }

        public bool IsAnEnemy(IBattleUnit targetUnit)
        {
            return this.TeamIndex != targetUnit.TeamIndex;
        }
    }
}
