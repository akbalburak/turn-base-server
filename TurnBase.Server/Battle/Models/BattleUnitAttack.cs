using TurnBase.Server.Battle.Core.Skills;
using TurnBase.Server.Battle.DTO;

namespace TurnBase.Server.Battle.Models
{
    public abstract class BattleUnitAttack : BattleUnit
    {
        private static Random random = new Random();

        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public float AttackSpeed { get; private set; }
        
        public List<BaseBattleSkill> Skills { get; set; }

        protected BattleUnitAttack(
            int health,
            int position,
            int minDamage,
            int maxDamage,
            float attackSpeed) :
            base(health, position)
        {
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
            this.AttackSpeed = attackSpeed;
            this.Skills = new List<BaseBattleSkill>();
        }


        public int GetDamage(BattleUnitAttack targetUnit)
        {
            return random.Next(this.MinDamage, this.MaxDamage + 1);
        }

        public void Attack(BattleUnit attackerUnit, int damage)
        {
            this.ReduceHealth(damage);
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
}
