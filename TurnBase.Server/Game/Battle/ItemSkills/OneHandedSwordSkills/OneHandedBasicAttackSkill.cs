﻿using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.Interfaces;

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

        public override void OnSkillUse(BattleSkillUseDTO useData)
        {
            // WE GET TARGET UNIT IN NODE.
            IBattleUnit targetUnit = Battle.GetUnitInNode(useData.TargetNodeIndex);
            if (targetUnit == null || !targetUnit.IsAnEnemy(Owner))
                return;

            // SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this);

            // WE DO THE SLASH.
            int damage = Owner.GetBaseDamage(targetUnit);
            Owner.AttackToUnit(targetUnit, damage);
            usageData.AddToDamage(targetUnit.UnitData.UniqueId, damage);

            // SEND TO USER.
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);
        }
    }
}