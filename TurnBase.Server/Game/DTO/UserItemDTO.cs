using Newtonsoft.Json;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Enums;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Game.Trackables;

namespace TurnBase.Server.Game.DTO
{
    public class UserItemDTO : TrackableDTO, IUserItemDTO
    {
        [JsonProperty("A")] public int UserItemID { get; set; }
        [JsonProperty("B")] public int ItemID { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public bool IsNew { get; set; }
        [JsonProperty("E")] public bool Equipped { get; set; }
        [JsonProperty("F")] public float Quality { get; set; }
        [JsonProperty("H")] public int Level { get; set; }
        [JsonProperty("I")] public int[] SelectedSkills { get; set; }

        public UserItemDTO()
        {
            SelectedSkills = Array.Empty<int>();
        }

        public override SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.InventoryModified, this);
        }

        public void RemoveQuantity(int quantity)
        {
            Quantity -= quantity;
            this.SetAsModified();
        }
        public void UpdateEquipState(bool isEquipped)
        {
            Equipped = isEquipped;
            SetAsModified();
        }
        public void ChangeActiveSkill(int row, int col)
        {
            // IF ROW IS INVALID RETURN.
            if (row >= SelectedSkills.Length)
                return;

            // WE UPDATE THE SELECTED ROW WITH THE NEW COL.
            SelectedSkills[row] = col;
            SetAsModified();
        }

        public bool TryGetSelectedSkillCol(int row, out int selectedSkillCol)
        {
            selectedSkillCol = -1;
            if (row >= SelectedSkills.Length)
                return false;

            selectedSkillCol = SelectedSkills[row];
            return true;
        }

    }
}
