using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.Skills
{
    public interface IItemSkill
    {
        int UniqueId { get; }
        public IItemSkillMappingDTO ItemSkill { get; }

        int LeftTurnToUse { get; }
        int TurnCooldown { get; }
        IBattleUnit Owner { get; }
        bool FinalizeTurnInUse { get; }

        bool IsSkillReadyToUse();
        void UseSkill(BattleSkillUseDTO useData);
    }
}
