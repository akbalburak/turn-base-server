using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkills.Base
{
    public abstract class BaseItemConsumableSkill : BaseItemSkill, IItemConsumableSkill
    {
        public int UsageCount { get; private set; }
        public IInventoryItemDTO InventoryItem { get; private set; }
        public int LeftUseCount { get; private set; }

        public BaseItemConsumableSkill(int uniqueId,
                                       IItemSkillDTO skill,
                                       IBattleItem battle,
                                       IBattleUnit owner,
                                       float itemQuality,
                                       IInventoryItemDTO inventoryItem)
            : base(uniqueId, skill, battle, owner, itemQuality)
        {
            InventoryItem = inventoryItem;

            int maxCount = skill.GetDataValueAsInt(ItemSkillData.Consumable, inventoryItem.Quality);
            LeftUseCount = Math.Min(inventoryItem.Quantity, maxCount);
        }

        public override bool IsSkillReadyToUse()
        {
            return base.IsSkillReadyToUse() && LeftUseCount > 0;
        }

        protected override BattleSkillUsageDTO OnSkillUsing(BattleSkillUseDTO useData)
        {
            LeftUseCount = Math.Max(LeftUseCount - 1, 0);
            UsageCount++;

            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);
            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.ConsumableUseCount, 1);
            return usageData;
        }

        public override BattleSkillDTO GetSkillDataDTO()
        {
            return new BattleSkillDTO(this);
        }
    }
}
