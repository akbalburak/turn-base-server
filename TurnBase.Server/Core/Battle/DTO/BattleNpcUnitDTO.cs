using Newtonsoft.Json;

namespace TurnBase.Server.Core.Battle.DTO
{
    public class BattleNpcUnitDTO : BattleUnitDTO
    {
        [JsonProperty("Z")] public int UnitId { get; set; }
    }
}
