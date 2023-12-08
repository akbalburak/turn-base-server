using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleInventory
    {
        IInventoryItemDTO[] IItems { get; }

        void AddItem(IBattleDropItem item);
    }
}
