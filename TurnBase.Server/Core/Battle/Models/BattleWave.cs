namespace TurnBase.Server.Core.Battle.Models
{
    public class BattleWave
    {
        public BattleNpcUnit[] Units => _units;

        private BattleNpcUnit[] _units;

        public BattleWave(BattleNpcUnit[] units)
        {
            _units = units;
        }

    }
}
