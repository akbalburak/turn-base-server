using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Effects
{
    public class BleedingEffect : BaseEffect
    {
        public BleedingEffect(IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IEffectData effectData)
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
