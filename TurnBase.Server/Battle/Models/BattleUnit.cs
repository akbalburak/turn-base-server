namespace TurnBase.Server.Battle.Models
{
    public abstract class BattleUnit
    {
        public int Id { get; private set; }
        public int MaxHealth { get; }
        public int Health { get; private set; }
        public int Position { get; private set; }
        public bool IsDeath { get; set; }

        protected BattleUnit(int health, int position)
        {
            MaxHealth = health;
            Health = health;
            Position = position;
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
    }
}
