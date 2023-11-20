using TurnBase.Server.Core.Battle.Enums;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IEffect
    {
        BattleEffects Effect { get; }
        IBattleItem Battle { get; }
        IBattleUnit ByWhom { get; }
        IBattleUnit ToWhom { get; }
        int LeftTurnDuration { get; }
    }
}
