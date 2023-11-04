using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle
{
    public class BattleNpcUnit : BattleUnitAttack
    {
        public BattleUnits UnitType { get; }

        public BattleNpcUnit(int id, BattleUnits unit,
            int health,
            int position,
            int minDamage,
            int maxDamage,
            float attackSpeed)
            : base(id, health, position, minDamage, maxDamage, attackSpeed)
        {
            this.UnitType = unit;
        }
    }
}
