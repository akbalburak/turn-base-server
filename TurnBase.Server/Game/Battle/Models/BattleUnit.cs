﻿using TurnBase.Server.Game.Battle.DTO;
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

        public IBattleUnitData UnitData { get; private set; }
        public IAStarNode CurrentNode { get; private set; }
        public IAStarNode KilledNode { get; private set; }

        public int Health { get; private set; }
        public int Mana { get; private set; }

        public bool IsAggrieved { get; private set; }
        public bool IsDeath { get; private set; }

        public BattleUnitStats Stats { get; private set; }

        public List<IItemSkill> Skills { get; private set; }
        public List<IItemSkillEffect> Effects { get; private set; }

        public IBattleUnit KilledBy { get; private set; }

        protected BattleUnit()
        {
            Skills = new List<IItemSkill>();
            Effects = new List<IItemSkillEffect>();
            Stats = new BattleUnitStats();
        }

        public int GetBaseDamage(IBattleUnit targetUnit)
        {
            bool isCritical = UnitData.BattleItem.GetRandomValue <= Stats.CriticalChance;
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
            defender.HitUnit(attacker: this, damage);
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
        public IItemConsumableSkill[] GetConsumableSkills()
        {
            return this.Skills.Where(x => x is IItemConsumableSkill)
                .Cast<IItemConsumableSkill>()
                .ToArray();
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

        public bool IsAnEnemy(IBattleUnit targetUnit)
        {
            return this.UnitData.TeamIndex != targetUnit.UnitData.TeamIndex;
        }

        public bool IsManaEnough(int manaCost)
        {
            return Mana >= manaCost;
        }
        public void UseMana(int usageManaCost)
        {
            Mana -= usageManaCost;

            if (Mana < 0)
                Mana = 0;
        }

        public void HitUnit(IBattleUnit attacker, int reduction)
        {
            Health -= reduction;

            if (Health <= 0)
                Kill(attacker);
        }
        public void Kill(IBattleUnit killedBy)
        {
            if (IsDeath)
                return;

            KilledNode = CurrentNode;
            ChangeNode(null);

            IsDeath = true;
            KilledBy = killedBy;

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
        public virtual void SetUnitData(IBattleUnitData unitData)
        {
            UnitData = unitData;
            this.ChangeNode(UnitData.InitialNode);
            this.LoadSkills();
        }

        public void OnAggrieving(IAStarUnit unit)
        {
            if (IsAggrieved)
                return;

            // IF THEY ARE IN THE SAME TEAM WONT AGGRO.
            if (unit.UnitData.TeamIndex == UnitData.TeamIndex)
                return;

            UnitData.BattleItem.CallGroupAggrieving(UnitData.GroupIndex);
        }
        public void OnAggrieved()
        {
            IsAggrieved = true;
        }

        public void UseAI()
        {
            IItemSkill[] readySkills = Skills
                .Where(x => x.IsSkillReadyToUse())
                .OrderBy(x => x.FinalizeTurnInUse)
                .OrderByDescending(x => x.InitialCooldown)
                .ToArray();

            for (int i = 0; i < readySkills.Length; i++)
            {
                IItemSkill skill = readySkills[i];

                // WE MAKE SURE THERE IS A VALID NODE.
                int? nodeIndex = skill.GetNodeIndexForAI();
                if (!nodeIndex.HasValue)
                    continue;

                // WE TRY TO USE SKILL.
                UseSkill(new BattleSkillUseDTO()
                {
                    UniqueSkillID = skill.UniqueId,
                    TargetNodeIndex = nodeIndex.Value
                });

                //IF NOT PLAYER TURN BREAK.
                if (!UnitData.BattleItem.BattleTurnHandler.IsUnitTurn(this))
                    break;
            }

            // IF STILL PLAYER TURN FINALIZE IT.
            if (UnitData.BattleItem.BattleTurnHandler.IsUnitTurn(this))
                UnitData.BattleItem.FinalizeTurn();
        }

        public void IncreaseHealth(int health)
        {
            Health += health;
            Health = Math.Min(Health, Stats.MaxHealth);
        }

        public void ChangeHealth(int newHealth)
        {
            Health = newHealth;
        }
    }
}
