namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IInventoryItemDTO : IStoreableItemDTO
    {
        int InventoryItemID { get; }
    }
}
