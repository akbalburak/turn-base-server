using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills;
using TurnBase.Server.Game.Battle.ItemSkills.PotionSkills;
using TurnBase.Server.Game.Battle.ItemSkills.SprintSkills;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.Skills
{
    public static class ItemSkillCreator
    {
        public static IItemSkill CreateSkill(int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            float itemQuality,
            IInventoryItemDTO inventoryItem = null)
        {
            return skill.ItemSkill switch
            {
                Enums.ItemSkills.DoubleSlash => new DoubleSlashSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.BleedingSlash => new BleedingSlashSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.FinishHim => new FinishHimSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.SplashSlashSkill => new SplashSlashSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.BasicSprint => new BasicSprintSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.OneHandedBasicAttackSkill => new OneHandedBasicAttackSkill(uniqueId, skill, battle, owner, itemQuality),
                Enums.ItemSkills.SmallHealthPotionSkill => new HealthPotionSkill(uniqueId, skill, battle, owner, itemQuality, inventoryItem),
                _ => null,
            };
        }
    }
}
