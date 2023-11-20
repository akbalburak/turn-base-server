using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Effects
{
    public static class EffectCreator
    {
        public static ISkillEffect GetEffect(BattleEffects effect,
            IBattleItem battle, IBattleUnit byWhom, IBattleUnit toWhom, ISkillEffectData effectData)
        {
            switch (effect)
            {
                case BattleEffects.Bleeding:
                    return new BleedingEffect(battle, byWhom, toWhom, effectData);
                default:
                    return null;
            }
        }
    }
}
