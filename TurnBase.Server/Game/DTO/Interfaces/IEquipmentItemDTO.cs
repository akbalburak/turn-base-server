using Newtonsoft.Json;

namespace TurnBase.Server.Game.DTO.Interfaces
{
    public interface IEquipmentItemDTO : IInventoryItemDTO
    {
        bool Equipped { get; }

        void ChangeActiveSkill(int rowIndex, int colIndex);
        void UpdateEquipState(bool isEquipped);

        bool TryGetSelectedSkillCol(int row, out int selectedSkillCol);

        void SetAsModified();
    }
}
