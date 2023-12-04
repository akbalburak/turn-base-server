using TurnBase.Server.Game.Battle.Enums;

namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IItemSkillMappingDTO
    {
        public ItemSkills ItemSkill { get; }
        public int RowIndex { get; }
        public int ColIndex { get; }
    }
}
