using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Interfaces;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public class DoubleSlashSkill : BaseSkill
    {
        public DoubleSlashSkill(int uniqueId, IItemSkillDTO skill, IBattleItem battle, IBattleUnit unit, IUserItemDTO userItem, IItemDTO itemData)
            : base(uniqueId, skill, battle, unit, userItem, itemData)
        {
        }

        public override void OnSkillUse(BattleSkillUseDTO useData)
        {
            // WE ARE LOOKING FOR THE TARGET.
            IBattleUnit targetUnit = Battle.GetUnit(useData.TargetUnitID);
            if (targetUnit == null || targetUnit.IsDeath)
            {
                // WE ARE LOOKING FOR A RANDOM ENEMY TO ATTACK.
                targetUnit = Battle.GetAliveEnemyUnit(Owner);
                if (targetUnit == null)
                    return;
            }

            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);

            // WE DO THE FIRST SLASH.
            int damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // WE DO THE SECOND SLASH.
            damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);
        }
    }
}
