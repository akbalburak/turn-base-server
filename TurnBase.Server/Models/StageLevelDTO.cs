using Newtonsoft.Json;
using TurnBase.Server.Enums;

namespace TurnBase.Server.Models
{
    public class StageLevelDTO
    {
        [JsonProperty("A")] public int Stage { get; set; }
        [JsonProperty("B")] public int Level { get; set; }
        [JsonProperty("C")] public LevelDifficulities CompletedDifficulites { get; set; }
        [JsonProperty("D")] public int PlayCount { get; set; }
        [JsonProperty("E")] public int CompletedCount { get; set; }

        public StageLevelDTO(int stage,int level)
        {
            this.Stage = stage;
            this.Level = level;
        }

        public bool IsDifficulityAlreadyCompleted(LevelDifficulities difficulity)
        {
            return CompletedDifficulites.HasFlag(difficulity);
        }

        public void CompleteDifficulitiy(LevelDifficulities difficulity)
        {
            if (IsDifficulityAlreadyCompleted(difficulity))
                return;

            CompletedDifficulites |= difficulity;
        }

        public void IncreasePlayCount()
        {
            this.PlayCount++;
        }
    }
}
