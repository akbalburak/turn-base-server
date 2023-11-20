namespace TurnBase.Server.Game.Interfaces
{
    public interface IUserItemDTO
    {
        int ItemID { get; }

        bool TryGetSlotValue(int index, out int selectedSlot);
    }
}
