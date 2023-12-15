﻿using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkills.OneHandedSwordSkills
{
    /// <summary>
    /// IF TARGET ENEMY KILLED AFTER ATTACK. SKILL COOLDOWN RESETS.
    /// </summary>
    public class FinishHimSkill : BaseItemSkill
    {
        public FinishHimSkill(int uniqueId,
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

            // SKILL DAMAGE TO HIT.
            int damage = SkillData.GetDataValueAsInt(ItemSkillData.Damage, SkillQuality);
            Owner.AttackToUnit(targetUnit, damage);

            // WE ADD ATTRIBUTES.
            base.AddAttribute(Enums.ItemSkillUsageAttributes.TargetUnitId, targetUnit.UnitData.UniqueId);
            base.AddAttribute(Enums.ItemSkillUsageAttributes.Damage, damage);

            // IF TARGET UNIT IS DEATH DONT START COOLDOWN.
            if (targetUnit.IsDeath)
            {
                base.AddAttribute(Enums.ItemSkillUsageAttributes.DontStartCooldown, true);
                ResetCooldown();
            }

            return base.OnSkillUsing(useData);
        }
    }
}
