using Newtonsoft.Json;

namespace TurnBase.Server.Game.Battle.DTO
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
