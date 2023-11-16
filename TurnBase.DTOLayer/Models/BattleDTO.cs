
using Newtonsoft.Json;
using TurnBase.DTOLayer.Enums;

namespace TurnBase.DTOLayer.Models
{
    public class BattleDTO
    {
        public class BattleRequestDTO
        {
            [JsonProperty("A")] public int StageIndex { get; set; }
            [JsonProperty("B")] public int LevelIndex { get; set; }
            [JsonProperty("C")] public LevelDifficulities Difficulity { get; set; }
        }

        public class BattleResponseDTO
        {

        }
    }
}
