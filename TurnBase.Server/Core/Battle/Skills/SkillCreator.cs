using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Interfaces;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Battle.Skills
{
    public static class SkillCreator
    {
        public static ISkill CreateSkill(int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            IUserItemDTO userItem,
            IItemDTO itemData)
        {
            return skill.SkillId switch
            {
                BattleSkills.DoubleSlash => new DoubleSlashSkill(uniqueId, skill, battle, owner, userItem, itemData),
                BattleSkills.BleedingSlash => new BleedingSlashSkill(uniqueId, skill, battle, owner, userItem, itemData),
                _ => null,
            };
        }
    }
}
