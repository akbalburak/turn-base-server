namespace TurnBase.Server.Battle.DTO
{
    public class BattleAttackDTO
    {
        public List<BattleAttackItemDTO> Attacks { get; private set; }
        public BattleAttackDTO()
        {
            Attacks = new List<BattleAttackItemDTO>();
        }

        public void AddAttack(BattleAttackItemDTO attack)
        {
            this.Attacks.Add(attack);
        }
    }

    public class BattleAttackItemDTO
    {
        public int AttackerID { get; set; }
        public int DefenderID { get; set; }
        public int Damage { get; set; }

        public BattleAttackItemDTO(int attackerId, int defenderId, int damage)
        {
            this.AttackerID = attackerId;
            this.DefenderID = defenderId;
            this.Damage = damage;
        }
    }
}
