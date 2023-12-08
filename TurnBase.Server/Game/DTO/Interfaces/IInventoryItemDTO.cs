namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IInventoryItemDTO
    {
        int ItemID { get; }
        float Quality { get; }
        int Quantity { get; }
        int Level { get; }
    }
}
