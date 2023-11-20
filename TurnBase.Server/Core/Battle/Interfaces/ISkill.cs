using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Interfaces;

namespace TurnBase.Server.Core.Battle.Skills
{
    public interface ISkill
    {
        int UniqueId { get; }
        public IItemSkillDTO ItemSkill { get; }

        int LeftTurnToUse { get; }
        int TurnCooldown { get; }
        IBattleUnit Owner { get; }
        bool FinalizeTurnInUse { get; }

        bool IsSkillReadyToUse();
        void UseSkill(BattleSkillUseDTO useData);
    }
}
