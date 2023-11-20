using TurnBase.Server.Core.Battle.Enums;

namespace TurnBase.Server.Interfaces
{
    public interface IItemSkillDTO
    {
        public BattleSkills SkillId { get; }
        int SlotIndex { get; }
    }
}
