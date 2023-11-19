using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public abstract class BaseBattleSkill
    {
        public int UniqueId { get; private set; }
        protected IBattleItem Battle { get; private set; }
        protected IBattleUnit Owner { get; private set; }

        public BattleSkills Skill { get; private set; }
        public bool FinalizeTurnInUse { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseBattleSkill(
            int id,
            BattleSkills skill,
            IBattleItem battle,
            IBattleUnit unit
        )
        {
            UniqueId = id;
            Skill = skill;

            Owner = unit;
            Battle = battle;

            SkillDTO skillData = SkillService.GetSkill(skill);
            TurnCooldown = skillData.TurnCooldown;
            FinalizeTurnInUse = skillData.FinalizeTurnInUse;

            Owner.OnUnitTurnStart += OnUnitTurnStarted;
        }

        public bool IsSkillReadyToUse()
        {
            return LeftTurnToUse <= 0;
        }
        public virtual void UseSkill(BattleSkillUseDTO useData)
        {
            LeftTurnToUse = TurnCooldown;
        }

        private void OnUnitTurnStarted()
        {
            if (LeftTurnToUse <= 0)
                return;

            LeftTurnToUse -= 1;
        }
    }
}
