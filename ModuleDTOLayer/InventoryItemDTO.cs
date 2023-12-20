namespace ModuleDTOLayer
{
    public class InventoryItemDTO : IInventoryItemDTO
    {
        public int InventoryItemID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public bool IsNew { get; set; }
        public bool Equipped { get; set; }
        public float Quality { get; set; }
        public int Level { get; set; }
        public int[] SelectedSkills { get; set; }

        public bool IsSkillSelected(int rowIndex, int colIndex)
        {
            if (rowIndex >= SelectedSkills.Length)
                return false;

            return SelectedSkills[rowIndex] == colIndex;
        }

        public void ReplaceWith(InventoryItemDTO newValues)
        {
            Quantity = newValues.Quantity;
            IsNew = newValues.IsNew;
            Equipped = newValues.Equipped;
            Quality = newValues.Quality;
            Level = newValues.Level;
            SelectedSkills = newValues.SelectedSkills;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }

    public interface IInventoryItemDTO : IInventoryAddDTO
    {
        int InventoryItemID { get; }

        bool IsNew { get; }

        bool Equipped { get; }

        void AddQuantity(int quantity);
        bool IsSkillSelected(int rowIndex, int colIndex);
    }

    public interface IInventoryAddDTO
    {
        int ItemID { get; }
        int Level { get; }
        float Quality { get; }
        int Quantity { get; }
    }
}
