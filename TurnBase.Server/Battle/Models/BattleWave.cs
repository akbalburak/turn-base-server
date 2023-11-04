namespace TurnBase.Server.Battle.Models
{
    public class BattleWave
    {
        public BattleNpcUnit[] Units => _units;

        private BattleNpcUnit[] _units;
        private int _idCounter = 0;

        public BattleWave(BattleNpcUnit[] units)
        {
            _units = units;

            // GIVING UNIQUE ID FOR EACH UNIT.
            foreach (BattleNpcUnit unit in _units)
                unit.SetId(++_idCounter);
        }

    }
}
