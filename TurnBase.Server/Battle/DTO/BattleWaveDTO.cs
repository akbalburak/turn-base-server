using Newtonsoft.Json;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleWaveDTO
    {
        [JsonProperty("A")] public BattleNpcUnitDTO[] Units { get; set; }
        public BattleWaveDTO()
        {
            Units = Array.Empty<BattleNpcUnitDTO>();
        }
    }
}
