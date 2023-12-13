using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketUser : IDisposable
    {
        Action OnUserTimeout { get; set; }

        ISocketUserData User { get; }

        void SendToClient(SocketResponse responseData);

        bool IsInBattle { get; }
        IBattleItem CurrentBattle { get; }
        void SetBattle(IBattleItem battle);
        void ClearBattle();
    }
}
