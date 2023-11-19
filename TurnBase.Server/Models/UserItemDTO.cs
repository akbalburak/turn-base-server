using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Server.ServerModels;
using TurnBase.Server.Trackables;

namespace TurnBase.Server.Models
{
    public class UserItemDTO : TrackableDTO
    {
        [JsonProperty("A")] public int UserItemID { get; set; }
        [JsonProperty("B")] public int ItemID { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public bool IsNew { get; set; }
        [JsonProperty("E")] public bool Equipped { get; set; }
        [JsonProperty("F")] public float Quality { get; set; }
        [JsonProperty("H")] public int Level { get; set; }
        [JsonProperty("I")] public int[] SkillSlots { get; set; }

        public UserItemDTO()
        {
            SkillSlots = Array.Empty<int>();
        }

        public override SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.InventoryModified, this);
        }

        public void UpdateEquipState(bool isEquipped)
        {
            this.Equipped = isEquipped;
            base.SetAsChanged();
        }
    }
}
