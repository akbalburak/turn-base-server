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
        private BattleUnitAttack[] _npcUnits;
        private BattleUnitAttack[] _playerUnits;
        private List<BattleTurnItem> _unitAttackTurns;
        private BattleTurnItem _currentTurn;
        private BattleTurnDTO _lastSentUnitTurnData;

        public BattleTurnHandler(
            BattleItem battle,
            BattleUnitAttack[] players, 
            BattleUnitAttack[] battleUnits)
        {
            this._battle = battle;

            _unitAttackTurns = new List<BattleTurnItem>();
            _lastSentUnitTurnData = new BattleTurnDTO();

            _playerUnits = players;

            AddUnits(battleUnits);
        }

        public bool IsUnitTurn(BattleUnitAttack unit)
        {
            return GetCurrentTurnUnit() == unit;
        }
        public BattleUnitAttack GetCurrentTurnUnit()
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

            foreach (BattleUnitAttack? battleUnit in _npcUnits.OrderByDescending(y => y.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));

            foreach (BattleUnitAttack? battleUnit in _playerUnits.OrderByDescending(y => y.AttackSpeed))
                _unitAttackTurns.Add(new BattleTurnItem(battleUnit));
        }

        public void RemoveUnits(BattleUnitAttack[] units)
        {
            _unitAttackTurns.RemoveAll(y => units.Contains(y.Unit));
        }
        public void AddUnits(BattleUnitAttack[] units)
        {
            _npcUnits = units;
            CalculateAttackOrder();
        }

        private class BattleTurnItem
        {
            public float BaseAttackTurn { get; }
            public float NextAttackTurn { get; private set; }
            public BattleUnitAttack Unit { get; }

            public BattleTurnItem(BattleUnitAttack unit)
            {
                Unit = unit;
                NextAttackTurn = unit.AttackSpeed;
                BaseAttackTurn = unit.AttackSpeed;
            }

            public void UpdateNextAttack()
            {
                NextAttackTurn += Unit.AttackSpeed;
            }
        }
    }
}
