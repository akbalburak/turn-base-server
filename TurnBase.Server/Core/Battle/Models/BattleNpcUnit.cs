namespace TurnBase.Server.Core.Battle.Models
{
    public class BattleNpcUnit : BattleUnit
    {
        public int UnitId { get; }
        public BattleNpcUnit(int unitId, int position, BattleUnitStats stats)
            : base(position)
        {
            UnitId = unitId;
            base.LoadStats(stats);
        }
    }
}
