using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleUnitMoveRequestDTO
    {
        [JsonProperty("A")] public float PosX { get; set; }
        [JsonProperty("B")] public float PosZ { get; set; }
    }

    public class BattleUnitMoveResponseDTO
    {
        [JsonProperty("A")] public int UnitUniqueId { get; set; }
        [JsonProperty("B")] public float PosX { get; set; }
        [JsonProperty("C")] public float PosZ { get; set; }
    }
}
