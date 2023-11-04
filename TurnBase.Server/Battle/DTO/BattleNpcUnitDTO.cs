using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleNpcUnitDTO : BattleUnitDTO
    {
        [JsonProperty("Z")] public BattleUnits Unit { get; set; }
    }
}
