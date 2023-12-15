using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Enums;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IItemSkillEffect
    {
        Action<IItemSkillEffect> OnEffectCompleted { get; set; }

        IDictionary<ItemSkillEffectAttributes, object> GetTempAttributes();

        BattleEffects Effect { get; }
        IBattleItem Battle { get; }
        IBattleUnit ByWhom { get; }
        IBattleUnit ToWhom { get; }
        int LeftTurnDuration { get; }
        bool IsFriendEffect { get; }

        BattleEffectStartedDTO GetEffectStartDTO();
    }
}
