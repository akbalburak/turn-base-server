using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleTurnDTO
    {
        [JsonProperty("A")] public int UnitId { get; set; }
        public BattleTurnDTO(int unitId)
        {
            UnitId = unitId;
        }

    }
}
