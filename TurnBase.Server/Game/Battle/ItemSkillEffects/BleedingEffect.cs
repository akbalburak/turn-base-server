using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Base;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects
{
    public class BleedingEffect : BaseEffect
    {
        public BleedingEffect(IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            float itemQuality
        )
            : base(BattleEffects.Bleeding, 
                  battle, 
                  byWhom, 
                  toWhom, 
                  skill,
                  itemQuality)
        {
        }

        protected override BattleEffectTurnExecutionDTO OnEffectExecuting()
        {
            int damage = base.Skill.GetDataValueAsInt(ItemSkillData.Damage, base.EffectQuality);

            // DO THE ACTION.
            ToWhom.ReduceHealth(damage);

            // RETURN REQUIRED DATA.
            BattleEffectTurnExecutionDTO effectExecuteData = new BattleEffectTurnExecutionDTO(this)
            {
                Damage = damage
            };

            return effectExecuteData;
        }
    }
}
