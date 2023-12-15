using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    public class OneHandedBasicAttackSkill : BaseItemSkill
    {
        public OneHandedBasicAttackSkill(int uniqueId,
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

            // WE MAKE A BASIC ATTACK.
            int damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);

            // WE ADD ATTRIBUTES TO SEND CLIENTS.
            base.AddAttribute(Enums.ItemSkillUsageAttributes.TargetUnitId, targetUnit.UnitData.UniqueId);
            base.AddAttribute(Enums.ItemSkillUsageAttributes.Damage, damage);

            return base.OnSkillUsing(useData);
        }

        public override int? GetNodeIndexForAI()
        {
            IBattleUnit enemy = Battle.GetAliveEnemyUnit(Owner, 1);
            if (enemy == null) 
                return null;

            return Battle.GetNodeIndex(enemy.CurrentNode);
        }
    }
}
