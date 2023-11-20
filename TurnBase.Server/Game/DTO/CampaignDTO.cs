using Newtonsoft.Json;
using TurnBase.Server.Game.Enums;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.DTO
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
            StageLevelDTO levelProgress = GetStageProgress(stageIndex, levelIndex);
            if (levelProgress == null)
                return false;

            return levelProgress.IsDifficulityAlreadyCompleted(difficulity);
        }
        public void AddStageProgress(int stageIndex, int levelIndex, LevelDifficulities difficulity)
        {
            StageLevelDTO progress = GetStageProgress(stageIndex, levelIndex);
            if (progress == null)
            {
                StageProgress.Add(new StageLevelDTO(stageIndex, levelIndex));

                progress = StageProgress[^1];

                progress.SetChangeHandler(_changeHandler);

                progress.CompleteDifficulitiy(difficulity);
            }
            else
            {
                if (!progress.IsDifficulityAlreadyCompleted(difficulity))
                    progress.CompleteDifficulitiy(difficulity);
            }

            progress.IncreasePlayCount();

            progress.SetAsChanged();
        }

        private StageLevelDTO GetStageProgress(int stageIndex, int levelIndex)
        {
            return StageProgress.Find(y => y.Stage == stageIndex &&
                                                y.Level == levelIndex);
        }

        private IChangeHandler _changeHandler;
        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            StageProgress.ForEach(e => e.SetChangeHandler(_changeHandler));
        }
    }
}
