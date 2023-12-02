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
        public IBattleItem Battle { get; private set; }
        public IBattleUnit Owner { get; private set; }

        public int UsageManaCost { get; private set; }
        public bool FinalizeTurnInUse { get; private set; }
        public int CurrentCooldown { get; private set; }
        public int InitialCooldown { get; private set; }

        public float SkillQuality { get; private set; }

        public BaseItemSkill(
            int uniqueId,
            IItemSkillDTO skill,
            IBattleItem battle,
            IBattleUnit owner,
            float itemQuality
        )
        {
            SkillQuality = itemQuality;

            UniqueId = uniqueId;
            Battle = battle;
            SkillData = skill;

            InitialCooldown = SkillData.GetDataValueAsInt(Game.Enums.ItemSkillData.Cooldown, SkillQuality);
            UsageManaCost = SkillData.GetDataValueAsInt(Game.Enums.ItemSkillData.ManaCost, SkillQuality);
            FinalizeTurnInUse = SkillData.FinalizeTurnInUse;

            Owner = owner;
            Owner.OnUnitTurnStart += OnUnitTurnStarted;
        }

        public virtual bool IsSkillReadyToUse()
        {
            return CurrentCooldown <= 0 && Owner.IsManaEnough(UsageManaCost);
        }

        public void UseSkill(BattleSkillUseDTO useData)
        {
            Owner.ReduceMana(UsageManaCost);

            CurrentCooldown = InitialCooldown;
            OnSkillUse(useData);

            if (!FinalizeTurnInUse)
                return;

            Battle.FinalizeTurn();
        }

        public abstract void OnSkillUse(BattleSkillUseDTO useData);

        protected void ResetCooldown()
        {
            CurrentCooldown = 0;
        }

        private void OnUnitTurnStarted(IBattleUnit unit)
        {
            if (CurrentCooldown <= 0)
                return;

            CurrentCooldown -= 1;
        }

        public virtual BattleSkillDTO GetSkillDataDTO()
        {
            return new BattleSkillDTO(this);
        }

        public virtual int? GetNodeIndexForAI()
        {
            return null;
        }
    }
}
