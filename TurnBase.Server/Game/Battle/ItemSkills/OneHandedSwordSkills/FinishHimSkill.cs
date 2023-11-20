using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    public class FinishHimSkill : BaseItemSkill
    {
        public FinishHimSkill(int uniqueId,
                              IItemSkillDTO skill,
                              IBattleItem battle,
                              IBattleUnit owner,
                              IUserItemDTO userItem,
                              IItemDTO itemData)
            : base(uniqueId, skill, battle, owner, userItem, itemData)
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

            // SKILL DAMAGE TO HIT.
            int damage = SkillData.GetDataValueAsInt(ItemSkillData.Damage, UserItem);

            // SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);

            // WE DO THE SLASH.
            Owner.AttackToUnit(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // IF TARGET UNIT IS DEATH DONT START COOLDOWN.
            if (targetUnit.IsDeath)
            {
                usageData.DontStartCooldown = true;
                ResetCooldown();
            }

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);
        }
    }
}
