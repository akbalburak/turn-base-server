﻿using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.DTO
{
    public class InventoryItemDTO : TrackableDTO, IEquipmentItemDTO
    {
        [JsonProperty("A")] public int InventoryItemID { get; set; }
        [JsonProperty("B")] public int ItemID { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public bool IsNew { get; set; }
        [JsonProperty("E")] public bool Equipped { get; set; }
        [JsonProperty("F")] public float Quality { get; set; }
        [JsonProperty("H")] public int Level { get; set; }
        [JsonProperty("I")] public int[] SelectedSkills { get; set; }

        public InventoryItemDTO()
        {
            SelectedSkills = Array.Empty<int>();
        }

        public override SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.InventoryModified, this);
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
