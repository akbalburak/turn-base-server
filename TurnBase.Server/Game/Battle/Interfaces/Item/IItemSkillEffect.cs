using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IItemSkillEffect
    {
        BattleEffects Effect { get; }
        IBattleItem Battle { get; }
        IBattleUnit ByWhom { get; }
        IBattleUnit ToWhom { get; }
        int LeftTurnDuration { get; }
    }
}
