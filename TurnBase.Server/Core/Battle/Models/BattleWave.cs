namespace TurnBase.Server.Core.Battle.Models
{
    public class BattleWave
    {
        public List<BattleNpcUnit> Units => _units;

        private List<BattleNpcUnit> _units;

        public BattleWave(List<BattleNpcUnit> units)
        {
            _units = units;
        }

    }
}
