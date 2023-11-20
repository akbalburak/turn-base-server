using TurnBase.Server.Game.Battle.Core.Skills;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Models;

namespace TurnBase.Server.Game.Battle.Skills
{
    public static class ItemSkillCreator
    {
        public static IItemSkill CreateSkill(int uniqueId,
            IItemSkillMappingDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            IUserItemDTO userItem,
            IItemDTO itemData)
        {
            return skill.SkillId switch
            {
                BattleSkills.DoubleSlash => new DoubleSlashItemSkill(uniqueId, skill, battle, owner, userItem, itemData),
                BattleSkills.BleedingSlash => new BleedingSlashItemSkill(uniqueId, skill, battle, owner, userItem, itemData),
                _ => null,
            };
        }
    }
}
