using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IBattleUser : IBattleUnit
    {
        IBattleInventory LootInventory { get; }
        ISocketUser SocketUser { get; }
        string PlayerName { get; }
        bool IsConnected { get; }
        int GetNewDataId { get; }
        int GetLastDataId { get; }
        bool IsFirstCompletion { get; }

        void UpdateSocketUser(ISocketUser socketUser);
    }
}
