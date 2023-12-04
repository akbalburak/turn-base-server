using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Interfaces.Item;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkills.Base
{
    public abstract class BaseItemStackableSkill : BaseItemSkill, IItemStackableSkill
    {
        public int InitialStackSize { get; private set; }
        public int CurrentStackSize { get; private set; }

        protected BaseItemStackableSkill(int uniqueId,
                                         IItemSkillDTO skill,
                                         IBattleItem battle,
                                         IBattleUnit owner,
                                         float itemQuality)
            : base(uniqueId, skill, battle, owner, itemQuality)
        {

            InitialStackSize = skill.GetDataValueAsInt(Game.Enums.ItemSkillData.Stack, base.SkillQuality);
            CurrentStackSize = InitialStackSize;

            Owner.OnUnitTurnStart += OnUnitTurnStarted;
        }

        private void OnUnitTurnStarted(IBattleUnit unit)
        {
            ResetStack();
        }

        protected bool IsStackEnough(int quantity)
        {
            return CurrentStackSize >= quantity;
        }

        protected void UseStack(int quantiy)
        {
            CurrentStackSize -= quantiy;
        }

        protected void ResetStack()
        {
            CurrentStackSize = InitialStackSize;
        }

        public override BattleSkillDTO GetSkillDataDTO()
        {
            return new BattleSkillDTO(this);
        }
    }
}
