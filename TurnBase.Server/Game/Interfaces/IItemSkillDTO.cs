using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillDTO
    {
        public ItemSkills ItemSkill { get; }

        bool FinalizeTurnInUse { get; }

        double GetDataValue(ItemSkillData data, float quality);
        int GetDataValueAsInt(ItemSkillData data, float quality);
    }
}
