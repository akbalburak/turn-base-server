﻿using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Buffs;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Debuffs;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects
{
    public static class EffectBuilder
    {
        public static void BuildEffect(
            BattleEffects effect,
            IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            float itemQuality
        )
        {
            // FOR DEATH ENEMY WE CANNOT CREATE ANY EFFECT.
            if (toWhom.IsDeath)
                return;

            switch (effect)
            {
                case BattleEffects.Bleeding:
                    _ = new BleedingEffect(battle, byWhom, toWhom, skill, itemQuality);
                    break;
                case BattleEffects.HealthBonus:
                    _ = new HealthBonusEffect(battle, byWhom, toWhom, skill, itemQuality);
                    break;
            }
        }
    }
}
