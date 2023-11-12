using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.Enums;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle.Core
{
    public class BattleTurnHandler
    {
        private BattleItem _battle;
        private BattleUnit[] _npcUnits;
        private BattleUnit[] _playerUnits;
        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;
        private BattleTurnDTO _lastSentUnitTurnData;

        public BattleTurnHandler(
            BattleItem battle,
            BattleUnit[] players,
            BattleUnit[] battleUnits)
        {
            this._battle = battle;

            _unitAttackTurns = new List<BattleTurnItem>();
            _lastSentUnitTurnData = new BattleTurnDTO();

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
                .FirstOrDefault(x=> !x.Unit.IsDeath);

            if (_currentTurn == null)
                return;

            // WE UPDATE THE LAST SENT DATA.
            _lastSentUnitTurnData.UnitId = _currentTurn.Unit.Id;

            // TURN CHANGED DATA.
            _battle.SendToAllUsers(BattleActions.TurnUpdated, _lastSentUnitTurnData);

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
