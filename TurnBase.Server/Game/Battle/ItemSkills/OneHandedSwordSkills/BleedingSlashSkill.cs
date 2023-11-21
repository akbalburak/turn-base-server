using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.ItemSkillEffects;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    public class BleedingSlashSkill : BaseItemSkill
    {
        public BleedingSlashSkill(int uniqueId,
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

            // SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);

            // WE DO THE SLASH.
            int damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UniqueId, damage);

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);

            // WE WILL CREATE A BLEEDING EFFECT.
            EffectBuilder.BuildEffect(BattleEffects.Bleeding,
                Battle,
                Owner,
                targetUnit,
                SkillData,
                UserItem
            );

        }
    }
}
