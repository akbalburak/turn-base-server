using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.ItemSkills;
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
                Enums.ItemSkills.DoubleSlash => new DoubleSlashSkill(uniqueId, skill, battle, owner, userItem, itemData),
                Enums.ItemSkills.BleedingSlash => new BleedingSlashSkill(uniqueId, skill, battle, owner, userItem, itemData),
                Enums.ItemSkills.FinishHim => new FinishHimSkill(uniqueId, skill, battle, owner, userItem, itemData),
                _ => null,
            };
        }
    }
}
