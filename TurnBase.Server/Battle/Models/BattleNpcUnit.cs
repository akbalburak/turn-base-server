using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle
{
    public class BattleNpcUnit : BattleUnitAttack
    {
        public int UnitId { get; }
        public BattleNpcUnit(int unitId, int health,
            int position,
            int minDamage,
            int maxDamage,
            float attackSpeed)
            : base(health, position, minDamage, maxDamage, attackSpeed)
        {
            this.UnitId = unitId;
        }
    }
}
