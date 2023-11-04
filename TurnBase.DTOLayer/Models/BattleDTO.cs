
using Newtonsoft.Json;

namespace TurnBase.DTOLayer.Models
{
    public class BattleDTO
    {
        public class BattleRequestDTO
        {
            [JsonProperty("A")] public int StageIndex { get; set; }
            [JsonProperty("B")] public int LevelIndex { get; set; }
        }

        public class  BattleResponseDTO
        {
            
        }
    }
}
