using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Effects
{
    public abstract class BaseEffect : ISkillEffect
    {
        public int LeftTurnDuration { get; set; }

        public BattleEffects Effect { get; set; }
        public IBattleItem Battle { get; set; }
        public IBattleUnit ByWhom { get; set; }
        public IBattleUnit ToWhom { get; set; }

        public BaseEffect(
            BattleEffects effect,
            IBattleItem battle,
            IBattleUnit byWhom,
            IBattleUnit toWhom,
            ISkillEffectData effectData
        )
        {
            this.Effect = effect;
            this.Battle = battle;
            this.ByWhom = byWhom;
            this.ToWhom = toWhom;

            this.LeftTurnDuration = effectData.TurnDuration;

            ByWhom.OnUnitTurnStart += OnUnitTurnStarted;
            ByWhom.OnUnitDie += OnUnitDie;

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
        }
    }
}
