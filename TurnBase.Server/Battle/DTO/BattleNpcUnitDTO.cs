using Newtonsoft.Json;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleNpcUnitDTO : BattleUnitDTO
    {
        [JsonProperty("Z")] public int UnitId { get; set; }
    }
}
