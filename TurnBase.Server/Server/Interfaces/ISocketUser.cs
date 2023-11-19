using TurnBase.Server.Core.Battle.Interfaces;
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
