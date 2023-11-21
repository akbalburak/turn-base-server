﻿using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Base;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects
{
    public class BleedingEffect : BaseEffect
    {
        public BleedingEffect(IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            IUserItemDTO userItem
        )
            : base(BattleEffects.Bleeding, 
                  battle, 
                  byWhom, 
                  toWhom, 
                  skill,
                  userItem)
        {
        }

        protected override BattleEffectTurnExecutionDTO OnEffectExecuting()
        {
            int damage = base.Skill.GetDataValueAsInt(ItemSkillData.Damage, base.UserItem);

            // DO THE ACTION.
            ToWhom.ReduceHealth(damage);

            // RETURN REQUIRED DATA.
            BattleEffectTurnExecutionDTO effectExecuteData = new BattleEffectTurnExecutionDTO(this)
            {
                Damage = damage
            };

            return effectExecuteData;
        }
    }
}