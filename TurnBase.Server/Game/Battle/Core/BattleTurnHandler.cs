using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Core
{
    public class BattleTurnHandler : IBattleTurnHandler
    {
        public Action<bool> OnInCombatStateChanged { get; set; }
        public bool IsInCombat { get; private set; }
        public IBattleTurnItem[] TurnItems => _unitAttackTurns.ToArray();

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
            int currentUnitId = 0;
            if (_currentTurn != null)
            {
                currentUnitId = _currentTurn.Unit.UnitData.UniqueId;
                _currentTurn.UpdateNextAttack();
            }

            _currentTurn = _unitAttackTurns
                .OrderBy(y => y.NextAttackTurn)
                .FirstOrDefault();

            if (_currentTurn == null)
                return;

            BattleTurnDTO unitTurnData = new BattleTurnDTO(_currentTurn.Unit.UnitData.UniqueId);
            _battle.SendToAllUsers(BattleActions.TurnUpdated, unitTurnData);

            // TURN CHANGED DATA.
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
        public void CalculateAttackOrder()
        {
            // WE FIND THE LATEST ATTACKER TO APPEND NEW UNITS.
            float currentMaxTurnSpeed = _unitAttackTurns
                .Select(x => x.NextAttackTurn - x.BaseAttackTurn)
                .DefaultIfEmpty()
                .Max();

            /// WE CHECK NEWLY ADDED UNITS.
            foreach (IBattleUnit battleUnit in _units.OrderByDescending(y => y.Stats.AttackSpeed))
            {
                if (_unitAttackTurns.Exists(y => y.Unit == battleUnit))
                    continue;

                _unitAttackTurns.Add(new BattleTurnItem(battleUnit, currentMaxTurnSpeed));
            }

            // WE SEND ALL PLAYERS TURN IS CHANGED.
            _battle.SendToAllUsers(BattleActions.TurnOrderChanged, new BattleTurnChangedDTO(this));

            // WE CHECK IF PLAYERS IN COMBAT.
            CheckIsInCombat();
        }

        private void CheckIsInCombat()
        {
            int teamCount = _unitAttackTurns.Select(x => x.Unit.UnitData.TeamIndex).Distinct().Count();
            bool isInCombat = teamCount > 1;

            if (IsInCombat == isInCombat)
                return;

            IsInCombat = isInCombat;
            OnInCombatStateChanged?.Invoke(isInCombat);

            _battle.SendToAllUsers(BattleActions.CombatStateChanged, new BattleCombatStateChangedDTO(this));
        }
        private void RemoveUnit(IBattleUnit unit)
        {
            unit.OnUnitDie -= OnUnitDie;
            _units.Remove(unit);

            BattleTurnItem unitTurnData = _unitAttackTurns.Find(y => y.Unit == unit);
            _unitAttackTurns.Remove(unitTurnData);

            // WE SEND ALL PLAYERS TURN IS CHANGED.
            _battle.SendToAllUsers(BattleActions.TurnOrderChanged, new BattleTurnChangedDTO(this));

            CheckIsInCombat();
        }
        private void OnUnitDie(IBattleUnit unit)
        {
            if (IsUnitTurn(unit))
                SkipToNextTurn();

            RemoveUnit(unit);
        }

        public interface IBattleTurnItem
        {
            public float BaseAttackTurn { get; }
            public float NextAttackTurn { get; }
            public IBattleUnit Unit { get; }
        }

        public class BattleTurnItem : IBattleTurnItem
        {
            public float BaseAttackTurn { get; }
            public float NextAttackTurn { get; private set; }
            public IBattleUnit Unit { get; }

            public BattleTurnItem(IBattleUnit unit, float offset = 0)
            {
                Unit = unit;
                NextAttackTurn = unit.Stats.AttackSpeed + offset;
                BaseAttackTurn = unit.Stats.AttackSpeed;
            }

            public void UpdateNextAttack()
            {
                NextAttackTurn += BaseAttackTurn;
            }

            public void ResetAndSkipTurn()
            {
                NextAttackTurn = 0;
            }
        }
    }
}
