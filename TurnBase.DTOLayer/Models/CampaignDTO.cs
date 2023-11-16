using Newtonsoft.Json;
using TurnBase.DTOLayer.Enums;

namespace TurnBase.DTOLayer.Models
{
    public class CampaignDTO
    {
        [JsonProperty("A")] public List<StageLevelDTO> StageProgress { get; set; }
        public CampaignDTO()
        {
            StageProgress = new List<StageLevelDTO>();
        }

        public bool IsDifficulityCompleted(int stageIndex, int levelIndex, LevelDifficulities difficulity)
        {
            var levelProgress = GetStageProgress(stageIndex, levelIndex);
            if (levelProgress == null)
                return false;

            return levelProgress.IsDifficulityAlreadyCompleted(difficulity);
        }

        public StageLevelDTO GetStageProgress(int stageIndex, int levelIndex)
        {
            return this.StageProgress.Find(y => y.Stage == stageIndex &&
                                                y.Level == levelIndex);
        }

        public void AddStageProgress(int stageIndex, int levelIndex, LevelDifficulities difficulity)
        {
            StageLevelDTO progress = GetStageProgress(stageIndex, levelIndex);
            if (progress == null)
            {
                this.StageProgress.Add(new StageLevelDTO(stageIndex, levelIndex));

                progress = this.StageProgress[^1];
                progress.CompleteDifficulitiy(difficulity);
            }
            else
            {
                if (!progress.IsDifficulityAlreadyCompleted(difficulity))
                    progress.CompleteDifficulitiy(difficulity);
            }

            progress.IncreasePlayCount();
        }
    }
}
