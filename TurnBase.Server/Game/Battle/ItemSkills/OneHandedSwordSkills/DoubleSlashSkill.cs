using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    /// <summary>
    /// MAKE A DOUBLE SLASH AT ONCE.
    /// </summary>
    public class DoubleSlashSkill : BaseItemSkill
    {
        public DoubleSlashSkill(int uniqueId,
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

            // WE DO THE SECOND SLASH.
            damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);

            // WE MAKE DOUBLE HIT.
            base.AddAttribute(Enums.ItemSkillUsageAttributes.TargetUnitId, targetUnit.UnitData.UniqueId);
            base.AddAttribute(Enums.ItemSkillUsageAttributes.Damage, new int[] { damage, damage });

            return base.OnSkillUsing(useData);
        }
    }
}
