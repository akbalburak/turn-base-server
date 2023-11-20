using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Skills;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.Services;

namespace TurnBase.Server.Game.Battle.Core.Skills
{
    public abstract class BaseItemSkill : IItemSkill
    {
        public int UniqueId { get; private set; }

        public IItemSkillDTO SkillData { get; private set; }
        public IUserItemDTO UserItem { get; private set; }
        public IItemDTO ItemData { get; private set; }
        public IItemSkillMappingDTO ItemSkill { get; private set; }
        public IBattleItem Battle { get; private set; }
        public IBattleUnit Owner { get; private set; }


        public bool FinalizeTurnInUse { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseItemSkill(
            int uniqueId,
            IItemSkillMappingDTO skill,
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

            SkillData = ItemSkillService.GetSkill(skill.SkillId);
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
