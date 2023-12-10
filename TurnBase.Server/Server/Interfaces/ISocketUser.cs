using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketUser : IDisposable
    {
        Action OnUserTimeout { get; set; }

        IBattleItem CurrentBattle { get; }
        ISocketUserData User { get; }

        void SendToClient(SocketResponse responseData);
        void SetBattle(IBattleItem battle);
        void ClearBattle();
    }
}
