namespace TurnBase.Server.Game.Interfaces
{
    public interface IInventoryItemDTO
    {
        int ItemID { get; }
        bool Equipped { get; }
        float Quality { get; }
        int Quantity { get; }

        void ChangeActiveSkill(int rowIndex, int colIndex);
        void UpdateEquipState(bool isEquipped);

        bool TryGetSelectedSkillCol(int row, out int selectedSkillCol);

        void SetAsModified();
    }
}
