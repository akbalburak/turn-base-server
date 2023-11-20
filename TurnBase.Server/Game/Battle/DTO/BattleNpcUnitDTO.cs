using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleNpcUnitDTO : BattleUnitDTO
    {
        [JsonProperty("Z")] public int UnitId { get; set; }
    }
}
