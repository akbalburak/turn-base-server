using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    public class SplashSlashSkill : BaseItemSkill
    {
        public SplashSlashSkill(int uniqueId,
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
            IBattleUnit targetUnit = Battle.GetUnitInNode(useData.TargetNodeIndex);
            if (targetUnit == null || !targetUnit.IsAnEnemy(Owner))
                return null;

            // WE DO THE FIRST SLASH.
            int damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);

            // WE PUT ATTRIBUTES.
            BattleSkillUsageDTO usageData = base.OnSkillUsing(useData);
            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.TargetUnitId, targetUnit.UnitData.UniqueId);
            usageData.AddAttribute(Enums.ItemSkillUsageAttributes.Damage, damage);
            return usageData;
        }
    }
}

