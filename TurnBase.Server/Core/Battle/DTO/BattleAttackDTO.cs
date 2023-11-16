using Newtonsoft.Json;

namespace TurnBase.Server.Core.Battle.DTO
{
    public class BattleAttackDTO
    {
        [JsonProperty("A")] public List<BattleAttackItemDTO> Attacks { get; private set; }
        public BattleAttackDTO()
        {
            Attacks = new List<BattleAttackItemDTO>();
        }

        public void AddAttack(BattleAttackItemDTO attack)
        {
            Attacks.Add(attack);
        }
    }

    public class BattleAttackItemDTO
    {
        [JsonProperty("A")] public int AttackerID { get; set; }
        [JsonProperty("B")] public int DefenderID { get; set; }
        [JsonProperty("C")] public int Damage { get; set; }

        public BattleAttackItemDTO(int attackerId, int defenderId, int damage)
        {
            AttackerID = attackerId;
            DefenderID = defenderId;
            Damage = damage;
        }
    }
}
