using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Core
{
    public class BattleTurnHandler : IBattleTurnHandler
    {
        private IBattleItem _battle;
        private IBattleUnit[] _npcUnits;
        private IBattleUnit[] _playerUnits;
        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;

        public BattleTurnHandler(
            IBattleItem battle,
            IBattleUnit[] players,
            IBattleUnit[] battleUnits)
        {
            _battle = battle;

            _unitAttackTurns = new List<BattleTurnItem>();

            _playerUnits = players;
            _npcUnits = battleUnits;

            CalculateAttackOrder();
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

            foreach (IBattleUnit battleUnit in _npcUnits.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));

            foreach (IBattleUnit battleUnit in _playerUnits.OrderByDescending(y => y.Stats.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));
        }

        public void RemoveUnits(IBattleUnit[] units)
        {
            _unitAttackTurns.RemoveAll(y => units.Contains(y.Unit));
        }
        public void AddUnits(IBattleUnit[] units)
        {
            _npcUnits = units;
            CalculateAttackOrder();
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
