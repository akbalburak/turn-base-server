﻿using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkills.Base;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkills.PotionSkills
{
    public class HealthPotionSkill : BaseItemConsumableSkill
    {
        public HealthPotionSkill(int uniqueId,
                                 IItemSkillDTO skill,
                                 IBattleItem battle,
                                 IBattleUnit owner,
                                 float itemQuality,
                                 IInventoryItemDTO inventoryItem)
            : base(uniqueId, skill, battle, owner, itemQuality, inventoryItem)
        {
        }

        public override void OnSkillUse(BattleSkillUseDTO useData)
        {
            base.OnSkillUse(useData);

            // WE RECOVERY PLAYER HEALTH.
            int recoveryValue = SkillData.GetDataValueAsInt(ItemSkillData.Recovery, this.SkillQuality);
            Owner.IncreaseHealth(recoveryValue);

            // WE SEND SKILL USAGE DATA.
            BattleSkillUsageDTO usageData = new BattleSkillUsageDTO(this, useCount: 1);
            usageData.AddRecovery(Owner.UnitData.UniqueId, recoveryValue);
            Battle.SendToAllUsers(BattleActions.UnitUseSkill, usageData);
        }
    }
}
