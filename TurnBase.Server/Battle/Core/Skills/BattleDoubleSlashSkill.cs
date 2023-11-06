using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle.Core.Skills
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

            if (targetUnit is not BattleUnitAttack defender)
                return;

            if (Owner is not BattleUnitAttack attacker)
                return;

            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(
                Owner.Id,
                base.UniqueId,
                Skill);

            // WE DO THE FIRST SLASH.
            int damage = attacker.GetDamage(defender);
            defender.Attack(Owner, damage);
            usageData.AddToDamage(defender.Id, damage);

            // WE DO THE SECOND SLASH.
            damage = attacker.GetDamage(defender);
            defender.Attack(Owner, damage);
            usageData.AddToDamage(defender.Id, damage);

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);

            Battle.EndTurn();
        }
    }
}
