using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Skills;
using TurnBase.Server.Core.Services;
using TurnBase.Server.Interfaces;

namespace TurnBase.Server.Core.Battle.Core.Skills
{
    public abstract class BaseSkill : ISkill
    {
        public int UniqueId { get; private set; }

        public ISkillDTO SkillData { get; private set; }
        public IUserItemDTO UserItem { get; private set; }
        public IItemDTO ItemData { get; private set; }
        public IItemSkillDTO ItemSkill { get; private set; }
        public IBattleItem Battle { get; private set; }
        public IBattleUnit Owner { get; private set; }


        public bool FinalizeTurnInUse { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseSkill(
            int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            IUserItemDTO userItem,
            IItemDTO itemData
        )
        {
            this.UniqueId = uniqueId;
            this.ItemData = itemData;
            this.UserItem = userItem;

            this.ItemSkill = skill;

            this.Battle = battle;

            SkillData = SkillService.GetSkill(skill.SkillId);
            this.TurnCooldown = SkillData.TurnCooldown;
            this.FinalizeTurnInUse = SkillData.FinalizeTurnInUse;

            this.Owner = owner;
            this.Owner.OnUnitTurnStart += OnUnitTurnStarted;
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
