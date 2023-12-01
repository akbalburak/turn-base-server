using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Interfaces;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects.Base
{
    public abstract class BaseEffect : IItemSkillEffect
    {
        public Action<IItemSkillEffect> OnEffectCompleted { get; set; }

        public bool IsFriendEffect { get; private set; }
        public int LeftTurnDuration { get; private set; }

        public BattleEffects Effect { get; private set; }
        public IBattleItem Battle { get; private set; }
        public IBattleUnit ByWhom { get; private set; }
        public IBattleUnit ToWhom { get; private set; }
        public IItemSkillDTO Skill { get; private set; }
        public float EffectQuality { get; set; }

        public BaseEffect(
            BattleEffects effect,
            IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            IItemSkillDTO skill,
            float itemQuality
        )
        {
            Effect = effect;
            Battle = battle;
            ByWhom = byWhom;
            Skill = skill;

            EffectQuality = itemQuality;


            LeftTurnDuration = skill.GetDataValueAsInt(ItemSkillData.Duration, EffectQuality);
            IsFriendEffect = byWhom.TeamIndex == toWhom.TeamIndex;

            ByWhom.OnUnitTurnStart += OnUnitTurnStarted;

            ToWhom = toWhom;
            ToWhom.OnUnitDie += OnUnitDie;
            ToWhom.AddEffect(this);

            OnEffectStarted();
        }

        private void OnUnitDie(IBattleUnit unit)
        {
            LeftTurnDuration = 0;
            OnEffectOver();
        }
        private void OnUnitTurnStarted(IBattleUnit unit)
        {
            LeftTurnDuration--;

            BattleEffectTurnExecutionDTO dataToSend = OnEffectExecuting();
            if (dataToSend != null)
            {
                Battle.SendToAllUsers(BattleActions.EffectExecutionTurn, dataToSend);
            }

            if (LeftTurnDuration > 0)
                return;

            OnEffectOver();
        }

        protected virtual void OnEffectStarted()
        {
            Battle.SendToAllUsers(BattleActions.EffectStarted, new BattleEffectStartedDTO(this));
        }

        protected abstract BattleEffectTurnExecutionDTO OnEffectExecuting();

        protected virtual void OnEffectOver()
        {
            ByWhom.OnUnitTurnStart -= OnUnitTurnStarted;
            ByWhom.OnUnitDie -= OnUnitDie;

            Battle.SendToAllUsers(BattleActions.EffectOver, new BattleEffectOverDTO(this));

            OnEffectCompleted?.Invoke(this);
        }
    }
}
