using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleTurnHandler
    {
        void AddUnits(IBattleUnit[] units);
        void RemoveUnits(IBattleUnit[] units);

        IBattleUnit GetCurrentTurnUnit();
        bool IsUnitTurn(IBattleUnit currentUser);

        void SkipToNextTurn();
    }
}
