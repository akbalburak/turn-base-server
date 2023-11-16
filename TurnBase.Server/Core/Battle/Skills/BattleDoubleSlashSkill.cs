using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public class BattleDoubleSlashSkill : BaseBattleSkill
    {
        public BattleDoubleSlashSkill(int id, BattleItem battle,
                                      BattleUnit unit)
            : base(id, BattleSkills.DoubleSlash, battle, unit, finalizeTurnInUse: true, 3)
        {
        }

        public override void UseSkill(BattleSkillUseDTO useData)
        {
            base.UseSkill(useData);

            // WE ARE LOOKING FOR THE TARGET.
            BattleUnit targetUnit = Battle.GetUnit(useData.TargetUnitID);
            if (targetUnit == null || targetUnit.IsDeath)
            {
                // WE ARE LOOKING FOR A RANDOM ENEMY TO ATTACK.
                targetUnit = Battle.GetAliveEnemyUnit(Owner);
                if (targetUnit == null)
                    return;
            }

            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(
                Owner.UniqueId,
                UniqueId,
                Skill);

            // WE DO THE FIRST SLASH.
            int damage = Owner.GetDamage(targetUnit);
            Owner.AttackTo(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // WE DO THE SECOND SLASH.
            damage = Owner.GetDamage(targetUnit);
            Owner.AttackTo(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);

            Battle.EndTurn();
        }
    }
}
