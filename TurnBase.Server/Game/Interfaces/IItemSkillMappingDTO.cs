using TurnBase.Server.Game.Battle.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillMappingDTO
    {
        public ItemSkills ItemSkill { get; }
        int SkillIndex { get; }
    }
}
