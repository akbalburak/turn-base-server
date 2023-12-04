using TurnBase.Server.Game.Battle.Models;
using static TurnBase.Server.Game.Battle.Core.BattleTurnHandler;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleTurnHandler
    {
        bool IsInCombat { get; }
        IBattleTurnItem[] TurnItems { get; }

        void AddUnits(IEnumerable<IBattleUnit> units);
        void RemoveUnits(IEnumerable<IBattleUnit> units);

        IBattleUnit GetCurrentTurnUnit();
        bool IsUnitTurn(IBattleUnit currentUser);

        void SkipToNextTurn();
        void CalculateAttackOrder();
    }
}
