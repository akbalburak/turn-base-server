using Newtonsoft.Json;
using TurnBase.DTOLayer.Enums;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleLoadAllDTO
    {
        [JsonProperty("A")] public BattleWaveDTO[] Waves { get; set; }
        [JsonProperty("B")] public BattlePlayerDTO[] Players { get; set; }
        [JsonProperty("C")] public LevelDifficulities Difficulity { get; set; }

        public BattleLoadAllDTO()
        {
            Waves = Array.Empty<BattleWaveDTO>();
            Players = Array.Empty<BattlePlayerDTO>();
        }
    }
}
