using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.Base
{
    public abstract class BaseItemSkill : IItemSkill
    {
        public int UniqueId { get; private set; }

        public IItemSkillDTO SkillData { get; private set; }
        public IUserItemDTO UserItem { get; private set; }
        public IItemDTO ItemData { get; private set; }
        public IBattleItem Battle { get; private set; }
        public IBattleUnit Owner { get; private set; }

        public int UsageManaCost { get; private set; }
        public bool FinalizeTurnInUse { get; private set; }
        public int LeftTurnToUse { get; private set; }
        public int TurnCooldown { get; private set; }

        public BaseItemSkill(
            int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            IUserItemDTO userItem,
            IItemDTO itemData
        )
        {
            UniqueId = uniqueId;
            ItemData = itemData;
            UserItem = userItem;

            Battle = battle;

            SkillData = skill;
            TurnCooldown = SkillData.TurnCooldown;
            UsageManaCost = SkillData.UsageManaCost;
            FinalizeTurnInUse = SkillData.FinalizeTurnInUse;

            Owner = owner;
            Owner.OnUnitTurnStart += OnUnitTurnStarted;
        }

        public virtual bool IsSkillReadyToUse()
        {
            return LeftTurnToUse <= 0 && Owner.IsManaEnough(UsageManaCost);
        }

        public void UseSkill(BattleSkillUseDTO useData)
        {
            Owner.ReduceMana(UsageManaCost);

            LeftTurnToUse = TurnCooldown;
            OnSkillUse(useData);

            if (!FinalizeTurnInUse)
                return;

            Battle.FinalizeTurn();
        }

        public abstract void OnSkillUse(BattleSkillUseDTO useData);

        protected void ResetCooldown()
        {
            LeftTurnToUse = 0;
        }

        private void OnUnitTurnStarted(IBattleUnit unit)
        {
            if (LeftTurnToUse <= 0)
                return;

            LeftTurnToUse -= 1;
        }
    }
}
