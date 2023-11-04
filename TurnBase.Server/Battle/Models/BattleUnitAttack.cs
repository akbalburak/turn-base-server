using TurnBase.Server.Battle.DTO;

namespace TurnBase.Server.Battle.Models
{
    public abstract class BattleUnitAttack : BattleUnit
    {
        private static Random random = new Random();

        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public float AttackSpeed { get; private set; }

        protected BattleUnitAttack(
            int id,
            int health,
            int position,
            int minDamage,
            int maxDamage,
            float attackSpeed) :
            base(id, health, position)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            AttackSpeed = attackSpeed;
        }


        public int GetDamage(BattleUnitAttack targetUnit)
        {
            return random.Next(this.MinDamage, this.MaxDamage + 1);
        }

        public void Attack(BattleUnit attackerUnit, int damage)
        {
            this.ReduceHealth(damage);
        }
    }
}
