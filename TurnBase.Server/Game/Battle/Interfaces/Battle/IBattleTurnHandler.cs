using TurnBase.Server.Game.Battle.Models;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleTurnHandler
    {
        void AddUnits(IEnumerable<IBattleUnit> units);
        void RemoveUnits(IEnumerable<IBattleUnit> units);

        IBattleUnit GetCurrentTurnUnit();
        bool IsUnitTurn(IBattleUnit currentUser);

        void SkipToNextTurn();
    }
}
