using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects.Buffs
{
    public class HealthBonusEffect : BaseEffect
    {
        private int _bonusHealth;
        public HealthBonusEffect(IBattleItem battle,
                                 IBattleUnit byWhom,
                                 IBattleUnit toWhom,
                                 IItemSkillDTO skill,
                                 float itemQuality)
            : base(BattleEffects.HealthBonus, battle, byWhom, toWhom, skill, itemQuality)
        {
        }

        protected override void OnEffectStarted()
        {
            _bonusHealth = Skill.GetDataValueAsInt(Game.Enums.ItemSkillData.HealthBonus, EffectQuality);

            ByWhom.Stats.IncreaseMaxHealth(_bonusHealth);
            ByWhom.IncreaseHealth(_bonusHealth);

            Attributes.Add(Enums.ItemSkillEffectAttributes.HealthBonus, _bonusHealth);

            base.OnEffectStarted();
        }
        protected override void OnEffectOver()
        {
            float healthRatio = Math.Clamp(ByWhom.Health / ByWhom.Stats.MaxHealth, 0, 1);

            ByWhom.Stats.DecreaseMaxHealth(_bonusHealth);

            int newHealth = (int)Math.Ceiling(healthRatio * ByWhom.Stats.MaxHealth);
            ByWhom.ChangeHealth(newHealth);

            Attributes.Add(Enums.ItemSkillEffectAttributes.HealthBonus, _bonusHealth);

            base.OnEffectOver();
        }
    }
}
