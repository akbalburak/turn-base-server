using TurnBase.Server.Core.Battle.Interfaces;

namespace TurnBase.Server.Core.Battle.Effects
{
    public class BaseEffectData : IEffectData
    {
        public int TurnDuration { get; private set; }
        public BaseEffectData(int turnDuration)
        {
            this.TurnDuration = turnDuration;
        }
    }
}
