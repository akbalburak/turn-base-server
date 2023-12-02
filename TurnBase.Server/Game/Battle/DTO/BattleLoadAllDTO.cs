using Newtonsoft.Json;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleLoadAllDTO
    {
        [JsonProperty("A")] public BattleNpcUnitDTO[] Units { get; set; }
        [JsonProperty("B")] public BattlePlayerDTO[] Players { get; set; }
        [JsonProperty("C")] public LevelDifficulities Difficulity { get; set; }

        public BattleLoadAllDTO()
        {
            Units = Array.Empty<BattleNpcUnitDTO>();
            Players = Array.Empty<BattlePlayerDTO>();
        }
    }
}
