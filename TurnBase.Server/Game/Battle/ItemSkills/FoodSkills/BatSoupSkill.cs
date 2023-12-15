using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.FoodSkills
{
    public class BatSoupSkill : BaseItemConsumableSkill
    {
        public BatSoupSkill(int uniqueId,
                            IItemSkillDTO skill,
                            IBattleItem battle,
                            IBattleUnit owner,
                            float itemQuality,
                            IInventoryItemDTO inventoryItem)
            : base(uniqueId, skill, battle, owner, itemQuality, inventoryItem)
        {
        }

        protected override BattleSkillUsageDTO OnSkillUsing(BattleSkillUseDTO useData)
        {
            // WE CREATE GIVEN EFFECT.
            EffectBuilder.BuildEffect(BattleEffects.HealthBonus,
                Battle,
                Owner,
                Owner,
                SkillData,
                SkillQuality
            );

            return base.OnSkillUsing(useData);
        }
    }
}
