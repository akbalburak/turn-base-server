using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Item
{
    public interface IItemSkill
    {
        int UniqueId { get; }

        IItemSkillDTO SkillData { get; }
        IBattleUnit Owner { get; }

        int LeftTurnToUse { get; }
        int TurnCooldown { get; }
        int UsageManaCost { get; }
        bool FinalizeTurnInUse { get; }

        bool IsSkillReadyToUse();
        void UseSkill(BattleSkillUseDTO useData);
    }
}
