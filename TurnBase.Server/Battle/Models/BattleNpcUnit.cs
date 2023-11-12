using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle
{
    public class BattleNpcUnit : BattleUnit
    {
        public int UnitId { get; }
        public BattleNpcUnit(int unitId, int position, UnitStats stats)
            : base(position, stats)
        {
            this.UnitId = unitId;
        }
    }
}
