using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Core
{
    public class BattleTurnHandler : IBattleTurnHandler
    {
        public bool IsInCombat => _isInCombat;

        private IBattleItem _battle;

        private List<IBattleUnit> _units;

        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;
        private bool _isInCombat;

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
            BattleTurnDTO unitTurnData = new BattleTurnDTO(_currentTurn.Unit.UnitData.UniqueId);
            _battle.SendToAllUsers(BattleActions.TurnUpdated, unitTurnData);

            _currentTurn.Unit.CallUnitTurnStart();
        }

        public void RemoveUnits(IEnumerable<IBattleUnit> units)
        {
            foreach (IBattleUnit unit in units)
                RemoveUnit(unit);
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

        private void CalculateAttackOrder()
        {
            _unitAttackTurns.Clear();

            foreach (IBattleUnit battleUnit in _units.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));

            // WE SKIP TO NEXT UNIT.
            if (_currentTurn != null)
            {
                _currentTurn = null;
                SkipToNextTurn();
            }

            // WE CHECK IF PLAYERS IN COMBAT.
            CheckIsInCombat();
        }
        private void CheckIsInCombat()
        {
            int teamCount = _unitAttackTurns.Select(x => x.Unit.UnitData.TeamIndex).Distinct().Count();
            bool isInCombat = teamCount > 1;

            if (_isInCombat == isInCombat)
                return;

            _isInCombat = isInCombat;
        }
        private void RemoveUnit(IBattleUnit unit)
        {
            unit.OnUnitDie -= OnUnitDie;
            _units.Remove(unit);

            BattleTurnItem unitTurnData = _unitAttackTurns.Find(y => y.Unit == unit);
            _unitAttackTurns.Remove(unitTurnData);

            CheckIsInCombat();
        }
        private void OnUnitDie(IBattleUnit unit)
        {
            if (IsUnitTurn(unit))
                SkipToNextTurn();

            RemoveUnit(unit);
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
