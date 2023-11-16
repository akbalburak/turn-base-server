using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Core
{
    public class BattleTurnHandler
    {
        private BattleItem _battle;
        private BattleUnit[] _npcUnits;
        private BattleUnit[] _playerUnits;
        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;

        public BattleTurnHandler(
            BattleItem battle,
            BattleUnit[] players,
            BattleUnit[] battleUnits)
        {
            _battle = battle;

            _unitAttackTurns = new List<BattleTurnItem>();

            _playerUnits = players;

            AddUnits(battleUnits);
        }

        public bool IsUnitTurn(BattleUnit unit)
        {
            return GetCurrentTurnUnit() == unit;
        }
        public BattleUnit GetCurrentTurnUnit()
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

            _currentTurn.Unit.CallTurnStart();
        }

        private void CalculateAttackOrder()
        {
            _unitAttackTurns.Clear();

            foreach (BattleUnit battleUnit in _npcUnits.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));

            foreach (BattleUnit battleUnit in _playerUnits.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));
        }

        public void RemoveUnits(BattleUnit[] units)
        {
            _unitAttackTurns.RemoveAll(y => units.Contains(y.Unit));
        }
        public void AddUnits(BattleUnit[] units)
        {
            _npcUnits = units;
            CalculateAttackOrder();
        }

        private class BattleTurnItem
        {
            public float BaseAttackTurn { get; }
            public float NextAttackTurn { get; private set; }
            public BattleUnit Unit { get; }

            public BattleTurnItem(BattleUnit unit)
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
