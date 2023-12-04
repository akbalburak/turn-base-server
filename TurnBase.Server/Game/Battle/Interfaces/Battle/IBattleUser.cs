using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IBattleUser : IBattleUnit
    {
        ISocketUser SocketUser { get; }
        string PlayerName { get; }
        bool IsConnected { get; }
        int GetNewDataId { get; }
        int GetLastDataId { get; }

        void UpdateSocketUser(ISocketUser socketUser);
    }
}
