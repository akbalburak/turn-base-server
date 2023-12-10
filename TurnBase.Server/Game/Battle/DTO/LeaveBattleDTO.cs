using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleDisconnectDTO
    {
        [JsonProperty("A")] public int UnitId { get; set; }
        public BattleDisconnectDTO(IBattleUser user)
        {
            UnitId = user.UnitData.UniqueId;
        }
    }

    public class BattleReconnectDTO
    {
        [JsonProperty("A")] public int UnitId { get; set; }
        public BattleReconnectDTO(IBattleUser user)
        {
            UnitId = user.UnitData.UniqueId;
        }
    }
}
