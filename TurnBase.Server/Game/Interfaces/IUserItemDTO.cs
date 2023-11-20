namespace TurnBase.Server.Game.Interfaces
{
    public interface IUserItemDTO
    {
        int ItemID { get; }
        bool Equipped { get; }
        float Quality { get; }

        bool TryGetSelectedSkillIndex(int index, out int selectedSlot);
    }
}
