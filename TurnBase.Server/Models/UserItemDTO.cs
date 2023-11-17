using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Models
{
    public class UserItemDTO : IChangeItem
    {
        [JsonProperty("A")] public int UserItemID { get; set; }
        [JsonProperty("B")] public int ItemID { get; set; }
        [JsonProperty("C")] public int Quantity { get; set; }
        [JsonProperty("D")] public bool IsNew { get; set; }
        [JsonProperty("E")] public bool Equipped { get; set; }
        [JsonProperty("F")] public float Quality { get; set; }
        [JsonProperty("H")] public int Level { get; set; }

        public SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.InventoryModified, this);
        }
    }
}
