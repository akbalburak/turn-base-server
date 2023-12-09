using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.DTO.Interfaces;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces
{
    public interface IBattleUser : IBattleUnit
    {
        IBattleInventory LootInventory { get; }
        IInventoryItemDTO[] Equipments { get; }

        ISocketUser SocketUser { get; }
        string PlayerName { get; }
        bool IsConnected { get; }
        int GetNewDataId { get; }
        int GetLastDataId { get; }
        bool IsFirstCompletion { get; }

        void UpdateSocketUser(ISocketUser socketUser);
    }
}
