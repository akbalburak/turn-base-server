using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Core
{
    public class BattleTurnHandler : IBattleTurnHandler
    {
        private IBattleItem _battle;

        private List<IBattleUnit> _units;

        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;

        public BattleTurnHandler(IBattleItem battle)
        {
            _battle = battle;

            _units = new List<IBattleUnit>();
            _unitAttackTurns = new List<BattleTurnItem>();

        }

        public bool IsUnitTurn(IBattleUnit unit)
        {
            return GetCurrentTurnUnit() == unit;
        }
        public IBattleUnit GetCurrentTurnUnit()
        {
            return _currentTurn?.Unit;
        }

        public void SkipToNextTurn()
        {
            _currentTurn?.UpdateNextAttack();

            _currentTurn = _unitAttackTurns
                .OrderBy(y => y.NextAttackTurn)
                .FirstOrDefault(x => !x.Unit.IsDeath);

            if (_currentTurn == null)
                return;

            // TURN CHANGED DATA.
            BattleTurnDTO unitTurnData = new BattleTurnDTO(_currentTurn.Unit.UniqueId);
            _battle.SendToAllUsers(BattleActions.TurnUpdated, unitTurnData);

            _currentTurn.Unit.CallUnitTurnStart();
        }

        private void CalculateAttackOrder()
        {
            _unitAttackTurns.Clear();

            foreach (IBattleUnit battleUnit in _units.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));
        }

        public void RemoveUnits(IEnumerable<IBattleUnit> units)
        {
            foreach (IBattleUnit unit in units)
            {
                unit.OnUnitDie -= OnUnitDie;
                _units.Remove(unit);

                BattleTurnItem unitTurnData = _unitAttackTurns.Find(y => y.Unit == unit);
                _unitAttackTurns.Remove(unitTurnData);
            }
        }
        public void AddUnits(IEnumerable<IBattleUnit> units)
        {
            foreach (IBattleUnit unit in units)
            {
                unit.OnUnitDie += OnUnitDie;
                _units.Add(unit);
            }

            CalculateAttackOrder();
        }

        private void OnUnitDie(IBattleUnit unit)
        {
            if (!IsUnitTurn(unit))
                return;

            SkipToNextTurn();
        }

        private class BattleTurnItem
        {
            public float BaseAttackTurn { get; }
            public float NextAttackTurn { get; private set; }
            public IBattleUnit Unit { get; }

            public BattleTurnItem(IBattleUnit unit)
            {
                Unit = unit;
                NextAttackTurn = unit.Stats.AttackSpeed;
                BaseAttackTurn = unit.Stats.AttackSpeed;
            }

            public void UpdateNextAttack()
            {
                NextAttackTurn += Unit.Stats.AttackSpeed;
            }
        }
    }
}
