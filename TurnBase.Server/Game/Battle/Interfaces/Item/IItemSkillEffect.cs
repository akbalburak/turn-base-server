using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IItemSkillEffect
    {
        Action<IItemSkillEffect> OnEffectCompleted { get; set; }

        BattleEffects Effect { get; }
        IBattleItem Battle { get; }
        IBattleUnit ByWhom { get; }
        IBattleUnit ToWhom { get; }
        int LeftTurnDuration { get; }
        bool IsFriendEffect { get; }

        BattleEffectStartedDTO GetEffectDataDTO();
    }
}
