using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillDTO
    {
        ItemSkills ItemSkill { get; }
        ItemSkillShapes Shape { get; }
        ItemSkillTargets Target { get; }
        
        bool FinalizeTurnInUse { get; }

        double GetDataValue(ItemSkillData data, float quality);
        int GetDataValueAsInt(ItemSkillData data, float quality);
    }
}
