using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillDTO
    {
        public ItemSkills ItemSkill { get; }

        int TurnCooldown { get; }
        bool FinalizeTurnInUse { get; }
        int UsageManaCost { get; }

        double GetDataValue(ItemSkillData data, IUserItemDTO userItem);
        int GetDataValueAsInt(ItemSkillData data, IUserItemDTO userItem);
    }
}
