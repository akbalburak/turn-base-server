using TurnBase.Server.Game.Battle.Core.Skills;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.Skills
{
    public static class ItemSkillCreator
    {
        public static IItemSkill CreateSkill(int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            IUserItemDTO userItem,
            IItemDTO itemData)
        {
            return skill.ItemSkill switch
            {
                Enums.ItemSkills.DoubleSlash => new DoubleSlashItemSkill(uniqueId, skill, battle, owner, userItem, itemData),
                Enums.ItemSkills.BleedingSlash => new BleedingSlashItemSkill(uniqueId, skill, battle, owner, userItem, itemData),
                _ => null,
            };
        }
    }
}
