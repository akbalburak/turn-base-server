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
        private List<BattleTurnItem> _attackTurns;
        private BattleTurnItem _currentTurn;
        private BattleTurnDTO _lastSentTurnData;

        public BattleTurnHandler(
            BattleItem battle,
            BattleUnitAttack[] players, 
            BattleUnitAttack[] battleUnits)
        {
            this._battle = battle;

            _attackTurns = new List<BattleTurnItem>();
            _lastSentTurnData = new BattleTurnDTO();

            _npcUnits = battleUnits;
            _playerUnits = players;

            CalculateAttacks();
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

            _currentTurn = _attackTurns
                .OrderBy(y => y.NextAttackTurn)
                .FirstOrDefault(x=> !x.Unit.IsDeath);

            if (_currentTurn == null)
                return;

            // WE UPDATE THE LAST SENT DATA.
            _lastSentTurnData.UnitId = _currentTurn.Unit.Id;

            // TURN CHANGED DATA.
            _battle.SendToAllUsers(BattleActions.TurnUpdated, _lastSentTurnData);

            _currentTurn.Unit.CallTurnStart();
        }

        private void CalculateAttacks()
        {
            foreach (BattleUnitAttack? battleUnit in _npcUnits.OrderByDescending(y => y.AttackSpeed))
                _attackTurns.Add(new BattleTurnItem(battleUnit));

            foreach (BattleUnitAttack? battleUnit in _playerUnits.OrderByDescending(y => y.AttackSpeed))
                _attackTurns.Add(new BattleTurnItem(battleUnit));
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
