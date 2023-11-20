using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.Effects
{
    public static class EffectCreator
    {
        public static IItemSkillEffect GetEffect(BattleEffects effect,
            IBattleItem battle, IBattleUnit byWhom, IBattleUnit toWhom, IItemSkillEffectData effectData)
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
