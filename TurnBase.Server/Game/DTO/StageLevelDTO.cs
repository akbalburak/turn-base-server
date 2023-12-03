using Newtonsoft.Json;
using TurnBase.Server.Enums;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Game.Trackables;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.DTO
{
    public class StageLevelDTO : TrackableDTO
    {
        [JsonProperty("A")] public int Stage { get; set; }
        [JsonProperty("B")] public int Level { get; set; }
        [JsonProperty("D")] public int PlayCount { get; set; }
        [JsonProperty("E")] public int CompletedCount { get; set; }

        public StageLevelDTO(int stage, int level)
        {
            Stage = stage;
            Level = level;
        }

        public void IncreasePlayCount()
        {
            PlayCount++;
        }

        public override SocketResponse GetResponse()
        {
            return SocketResponse.GetSuccess(ActionTypes.CampaignProgressModified, this);
        }
    }
}
