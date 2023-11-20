using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Skills;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public abstract class BaseSkill : ISkill
    {
        public int UniqueId { get; private set; }

        public IBattleItem Battle { get; private set; }
        public IBattleUnit Owner { get; private set; }

        public BattleSkills Skill { get; private set; }

        public bool FinalizeTurnInUse { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseSkill(
            int uniqueId,
            BattleSkills skill,
            IBattleItem battle,
            IBattleUnit unit
        )
        {
            UniqueId = uniqueId;
            Skill = skill;

            Owner = unit;
            Battle = battle;

            SkillDTO skillData = SkillService.GetSkill(skill);
            TurnCooldown = skillData.TurnCooldown;
            FinalizeTurnInUse = skillData.FinalizeTurnInUse;

            Owner.OnUnitTurnStart += OnUnitTurnStarted;
        }

        public virtual bool IsSkillReadyToUse()
        {
            return LeftTurnToUse <= 0;
        }

        public void UseSkill(BattleSkillUseDTO useData)
        {
            LeftTurnToUse = TurnCooldown;
            OnSkillUse(useData);

            if (!FinalizeTurnInUse)
                return;

            Battle.FinalizeTurn();
        }

        public abstract void OnSkillUse(BattleSkillUseDTO useData);

        private void OnUnitTurnStarted(IBattleUnit unit)
        {
            if (LeftTurnToUse <= 0)
                return;

            LeftTurnToUse -= 1;
        }
    }
}
