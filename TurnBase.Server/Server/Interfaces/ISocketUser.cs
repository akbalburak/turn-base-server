using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketUser : IDisposable
    {
        IBattleItem CurrentBattle { get; }
        ISocketUserData User { get; }

        void SendToClient(SocketResponse responseData);
        void SetBattle(IBattleItem battle);
    }
}
