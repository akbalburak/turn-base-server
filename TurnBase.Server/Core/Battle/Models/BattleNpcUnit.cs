namespace TurnBase.Server.Core.Battle.Models
{
    public class BattleNpcUnit : BattleUnit
    {
        public int UnitId { get; }
        public BattleNpcUnit(int unitId, int position, UnitStats stats)
            : base(position, stats)
        {
            UnitId = unitId;
        }
    }
}
