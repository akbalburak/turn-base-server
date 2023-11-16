using TurnBase.Server.Core.Battle.Core;
using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public abstract class BaseBattleSkill
    {
        public int UniqueId { get; private set; }
        public BattleSkills Skill { get; private set; }
        public bool FinalizeTurnInUse { get; private set; }
        protected BattleItem Battle { get; private set; }
        protected BattleUnit Owner { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseBattleSkill(
                            int id,
                            BattleSkills skill,
                            BattleItem battle,
                            BattleUnit unit,
                            bool finalizeTurnInUse,
                            int turnCooldown)
        {
            UniqueId = id;
            Skill = skill;
            TurnCooldown = turnCooldown;
            FinalizeTurnInUse = finalizeTurnInUse;
            Owner = unit;
            Battle = battle;

            Owner.OnTurnStart += OnUnitTurnStarted;
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
