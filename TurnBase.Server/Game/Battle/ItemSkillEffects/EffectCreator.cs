using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects
{
    public static class EffectCreator
    {
        public static IItemSkillEffect GetEffect(
            BattleEffects effect,
            IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            IUserItemDTO userItem
        )
        {
            return effect switch
            {
                BattleEffects.Bleeding => new BleedingEffect(battle, byWhom, toWhom, skill, userItem),
                _ => null,
            };
        }
    }
}
