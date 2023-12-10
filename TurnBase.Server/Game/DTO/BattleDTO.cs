using Newtonsoft.Json;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.DTO
{
    public class BattleDTO
    {
        public class BattleRequestDTO
        {
            [JsonProperty("A")] public int StageIndex { get; set; }
            [JsonProperty("B")] public int LevelIndex { get; set; }
        }
    }
}
