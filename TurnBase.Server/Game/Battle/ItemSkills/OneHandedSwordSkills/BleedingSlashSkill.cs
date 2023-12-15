using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.ItemSkillEffects;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    /// <summary>
    /// BLEED TARGET ENEMY AND CAUSE DAMAGE PER TURN.
    /// </summary>
    public class BleedingSlashSkill : BaseItemSkill
    {
        private IBattleUnit _targetUnit;

        public BleedingSlashSkill(int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            float itemQuality)
            : base(uniqueId, skill, battle, owner, itemQuality)
        {
        }

        protected override BattleSkillUsageDTO OnSkillUsing(BattleSkillUseDTO useData)
        {
            // WE GET TARGET UNIT IN NODE.
            _targetUnit = Battle.GetUnitInNode(useData.TargetNodeIndex);
            if (_targetUnit == null || !_targetUnit.IsAnEnemy(Owner))
                return null;

            // WE DO THE SLASH.
            int damage = Owner.GetBaseDamage(_targetUnit);
            Owner.AttackToUnit(_targetUnit, damage);

            // WE STORE ATTRIBUTES.
            BattleSkillUsageDTO usageData = base.OnSkillUsing(useData);
            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.TargetUnitId,_targetUnit.UnitData.UniqueId);
            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.Damage, damage);
            return usageData;
        }

        protected override void OnSkillUsed(BattleSkillUsageDTO usageData)
        {
            // WE WILL CREATE A BLEEDING EFFECT.
            EffectBuilder.BuildEffect(
                effect: BattleEffects.Bleeding,
                battle: Battle,
                byWhom: Owner,
                toWhom: _targetUnit,
                skill: SkillData,
                itemQuality: base.SkillQuality
            );

            _targetUnit = null;
        }
    }
}
