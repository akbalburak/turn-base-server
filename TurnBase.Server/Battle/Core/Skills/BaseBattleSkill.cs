using TurnBase.Server.Battle.DTO;
using TurnBase.Server.Battle.Enums;
using TurnBase.Server.Battle.Models;

namespace TurnBase.Server.Battle.Core.Skills
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
            this.UniqueId = id;
            this.Skill = skill;
            this.TurnCooldown = turnCooldown;
            this.FinalizeTurnInUse = finalizeTurnInUse;
            this.Owner = unit;
            this.Battle = battle;

            Owner.OnTurnStart += OnUnitTurnStarted;
        }

        public bool IsSkillReadyToUse()
        {
            return LeftTurnToUse <= 0;
        }
        public virtual void UseSkill(BattleSkillUseDTO useData)
        {
            this.LeftTurnToUse = TurnCooldown;
        }

        private void OnUnitTurnStarted()
        {
            if (this.LeftTurnToUse <= 0)
                return;

            this.LeftTurnToUse -= 1;
        }
    }
}
