using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.ItemSkillEffects.Enums;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.ItemSkillEffects.Base
{
    public abstract class BaseEffect : IItemSkillEffect
    {
        public Action<IItemSkillEffect> OnEffectCompleted { get; set; }

        protected Dictionary<ItemSkillEffectAttributes, object> Attributes { get; private set; }

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
            Attributes = new Dictionary<ItemSkillEffectAttributes, object>();

            Effect = effect;
            Battle = battle;
            ByWhom = byWhom;
            Skill = skill;

            EffectQuality = itemQuality;


            LeftTurnDuration = skill.GetDataValueAsInt(ItemSkillData.Duration, EffectQuality);
            IsFriendEffect = byWhom.UnitData.TeamIndex == toWhom.UnitData.TeamIndex;

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


            if (LeftTurnDuration > 0)
                return;

            OnEffectOver();
        }

        protected virtual void OnEffectStarted()
        {
            Battle.SendToAllUsers(BattleActions.EffectStarted, GetEffectStartDTO());
            Attributes.Clear();
        }
        protected virtual void OnEffectTurnOver()
        {
        }
        protected virtual void OnEffectOver()
        {
            ByWhom.OnUnitTurnStart -= OnUnitTurnStarted;
            ByWhom.OnUnitDie -= OnUnitDie;

            Battle.SendToAllUsers(BattleActions.EffectOver, GetEffectOverDTO());
            Attributes.Clear();

            OnEffectCompleted?.Invoke(this);
        }


        protected void SendTurnOverData()
        {
            Battle.SendToAllUsers(BattleActions.EffectExecutionTurn, GetEffectTurnOver());
            Attributes.Clear();
        }


        public BattleEffectStartedDTO GetEffectStartDTO()
        {
            return new BattleEffectStartedDTO(this);
        }
        public BattleEffectTurnExecutionDTO GetEffectTurnOver()
        {
            return new BattleEffectTurnExecutionDTO(this);
        }
        public BattleEffectOverDTO GetEffectOverDTO()
        {
            return new BattleEffectOverDTO(this);
        }

        public IDictionary<ItemSkillEffectAttributes, object> GetTempAttributes()
        {
            return Attributes.ToDictionary(x => x.Key, x => x.Value);
        }

    }
}
