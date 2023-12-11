using Newtonsoft.Json;
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

        public bool IsLevelAlreadyCompleted(int stageIndex, int levelIndex)
        {
            StageLevelDTO levelProgress = GetLevelProgress(stageIndex, levelIndex);
            if (levelProgress == null)
                return false;

            return levelProgress.CompletedCount > 0;
        }
        public void SaveLevelProgress(int stageIndex, int levelIndex, bool isCompleted)
        {
            StageLevelDTO progress = GetLevelProgress(stageIndex, levelIndex);
            if (progress == null)
            {
                progress = new StageLevelDTO(stageIndex, levelIndex);
                progress.SetChangeHandler(_changeHandler);

                StageProgress.Add(progress);
            }

            progress.IncreasePlayCount();

            if (isCompleted)
            {
                progress.IncreaseCompleteCount();
            }

            progress.SetAsModified();
        }

        private StageLevelDTO GetLevelProgress(int stageIndex, int levelIndex)
        {
            return StageProgress.Find(y => y.Stage == stageIndex && y.Level == levelIndex);
        }

        private IChangeHandler _changeHandler;
        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
            StageProgress.ForEach(e => e.SetChangeHandler(_changeHandler));
        }
    }
}
