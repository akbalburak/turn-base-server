using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleUnitMoveRequestDTO
    {
        [JsonProperty("A")] public int ToIndex { get; set; }
    }

    public class BattleUnitMoveResponseDTO
    {
        [JsonProperty("A")] public int UnitUniqueId { get; set; }
        [JsonProperty("B")] public int ToIndex { get; set; }
    }
}
