using TurnBase.Server.Game.Battle.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillMappingDTO
    {
        public BattleSkills SkillId { get; }
        int SlotIndex { get; }
    }
}
