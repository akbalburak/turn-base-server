using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleUser : IBattleUnit
    {
        ISocketUser SocketUser { get; }
        string PlayerName { get; }
        bool IsConnected { get; }
        int GetNewDataId { get; }
    }
}
