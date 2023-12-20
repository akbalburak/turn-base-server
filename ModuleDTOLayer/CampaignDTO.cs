using System.Collections.Generic;

namespace ModuleDTOLayer
{
    public class CampaignDTO : ICampaignDTO
    {
        public List<StageLevelDTO> StageProgress { get; set; }

        public bool IsStageCompleted(int stageIndex, int levelIndex)
        {
            StageLevelDTO levelData = this.StageProgress
                .Find(y => y.Stage == stageIndex && y.Level == levelIndex);

            if (levelData == null)
                return false;

            return levelData.PlayCount > 0;
        }

        public StageLevelDTO GetStagProgress(int stage, int level)
        {
            return StageProgress.Find(y => y.Stage == stage && y.Level == level);
        }

        public void RemoveStageProgress(StageLevelDTO stage)
        {
            StageProgress.Remove(stage);
        }
        public void AddStageProgress(StageLevelDTO stage)
        {
            StageProgress.Add(stage);
        }
    }

    public interface ICampaignDTO
    {
        StageLevelDTO GetStagProgress(int stage, int level);
        bool IsStageCompleted(int stageIndex, int levelIndex);
        void RemoveStageProgress(StageLevelDTO stage);
        void AddStageProgress(StageLevelDTO stage);
    }
}
