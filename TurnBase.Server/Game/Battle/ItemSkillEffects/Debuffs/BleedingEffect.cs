using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Base;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Enums;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects.Debuffs
{
    public class BleedingEffect : BaseEffect
    {
        private int _damage;
        public BleedingEffect(IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            float itemQuality
        )
            : base(BattleEffects.Bleeding, battle, byWhom, toWhom, skill, itemQuality)
        {
        }

        protected override void OnEffectStarted()
        {
            base.OnEffectStarted();

            _damage = Skill.GetDataValueAsInt(ItemSkillData.Damage, EffectQuality);
        }

        protected override void OnEffectTurnOver()
        {
            // DO THE ACTION.
            ToWhom.HitUnit(attacker: ByWhom, _damage);

            // WE ADD INFORMATIONS.
            Attributes.Add(ItemSkillEffectAttributes.Damage, _damage);

            // WE SEND OVER SERVER.
            base.SendTurnOverData();
        }
    }
}
