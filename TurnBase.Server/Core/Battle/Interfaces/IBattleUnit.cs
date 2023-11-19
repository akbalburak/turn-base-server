using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleUnit
    {
        int TeamIndex { get; }
        bool IsDeath { get; }
        int UniqueId { get; }
        UnitStats Stats { get; }

        void ReduceHealth(int damage);
    }
}
