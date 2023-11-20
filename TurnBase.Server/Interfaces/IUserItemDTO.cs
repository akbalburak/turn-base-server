namespace TurnBase.Server.Interfaces
{
    public interface IUserItemDTO
    {
        int ItemID { get; }

        bool TryGetSlotValue(int index, out int selectedSlot);
    }
}
