using TurnBase.Server.Battle.Core.Skills;
using TurnBase.Server.Battle.DTO;

namespace TurnBase.Server.Battle.Models
{
    public abstract class BattleUnit
    {
        private static Random _random = new Random();

        public int TeamIndex { get; set; }
        public int Id { get; private set; }
        public int MaxHealth { get; }
        public int Health { get; private set; }
        public int Position { get; private set; }
        public bool IsDeath { get; set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public float AttackSpeed { get; private set; }

        public List<BaseBattleSkill> Skills { get; set; }

        protected BattleUnit(int health, 
            int position,
            int minDamage, 
            int maxDamage,
            float attackSpeed)
        {
            this.MaxHealth = health;
            this.Health = health;
            this.Position = position;
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
            this.AttackSpeed = attackSpeed;
            this.Skills = new List<BaseBattleSkill>();
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
            return _random.Next(this.MinDamage, this.MaxDamage + 1);
        }

        public void AttackTo(BattleUnit defender,int damage)
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
}
