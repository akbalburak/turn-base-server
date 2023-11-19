using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Skills
{
    public interface ISkill
    {
        int UniqueId { get; }
        BattleSkills Skill { get; }

        int LeftTurnToUse { get; }
        int TurnCooldown { get; }
        IBattleUnit Owner { get; }
        bool FinalizeTurnInUse { get; }

        bool IsSkillReadyToUse();
        void UseSkill(BattleSkillUseDTO useData);
    }
}
