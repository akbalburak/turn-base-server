using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.Effects
{
    public class BleedingEffect : BaseEffect
    {
        public BleedingEffect(IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillEffectData effectData)
            : base(BattleEffects.Bleeding, battle, byWhom, toWhom, effectData)
        {
        }

        protected override BattleEffectTurnExecutionDTO OnEffectExecuting()
        {
            // DO THE ACTION.
            ToWhom.ReduceHealth(1);

            // RETUN REQUIRED DATA.
            BattleEffectTurnExecutionDTO effectExecuteData = new BattleEffectTurnExecutionDTO(this)
            {
                Damage = 1
            };

            return effectExecuteData;
        }
    }
}
