using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.Effects
{
    public class BaseEffectData : IItemSkillEffectData
    {
        public int TurnDuration { get; private set; }
        public BaseEffectData(int turnDuration)
        {
            this.TurnDuration = turnDuration;
        }
    }
}
