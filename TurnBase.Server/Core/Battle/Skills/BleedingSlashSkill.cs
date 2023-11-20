using TurnBase.Server.Core.Battle.Core.Skills;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Effects;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Interfaces;

namespace TurnBase.Server.Core.Battle.Skills
{
    public class BleedingSlashSkill : BaseSkill
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
            ISkillEffect effect = EffectCreator.GetEffect(BattleEffects.Bleeding,
                Battle,
                Owner,
                targetUnit,
                new BaseEffectData(turnDuration: 3)
            );
            targetUnit.AddEffect(effect);

        }
    }
}
