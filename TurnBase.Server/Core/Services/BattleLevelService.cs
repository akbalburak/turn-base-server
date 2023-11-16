using TurnBase.DTOLayer.Enums;
using TurnBase.DTOLayer.Models;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Extends.Json;
using static TurnBase.DTOLayer.Models.BattleDTO;

namespace TurnBase.Server.Core.Services
{
    public static class BattleLevelService
    {
        private static Dictionary<string, string> _levels
            = new Dictionary<string, string>();

        private static Dictionary<string, BattleDataResponseDTO> _levelData
            = new Dictionary<string, BattleDataResponseDTO>();

        public static void Initialize()
        {
            string[] files = Directory.GetFiles("Battle/Stages", string.Empty, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                if (!file.EndsWith(".json"))
                    continue;

                string fileData = File.ReadAllText(file);

                BattleLevelData levelData = fileData.ToObject<BattleLevelData>();
                if (levelData == null)
                    continue;

                levelData.Difficulities.ForEach(diffData =>
                {
                    string levelName = GetLevelKey(levelData.Stage, levelData.Level, diffData.Difficulity);
                    _levelData.Add(levelName, new BattleDataResponseDTO
                    {
                        FirstTimeRewards = diffData.FirstCompletionRewards.Select(y => new BattleDTO.BattleRewardItemData
                        {
                            ItemId = y.ItemId,
                            Level = y.Level,
                            Quality = y.Quality,
                            Quantity = y.Quantity,
                        }).ToList()
                    });
                });

                _levels.Add(GetLevelKey(levelData.Stage, levelData.Level), fileData);
            }
        }

        public static BattleLevelData GetLevelData(int stageIndex, int levelIndex)
        {
            string levelName = GetLevelKey(stageIndex, levelIndex);

            if (!_levels.TryGetValue(levelName, out string levelData))
                return null;

            return levelData.ToObject<BattleLevelData>();
        }

        public static BattleDataResponseDTO GetLevelMetaData(int stageIndex, int levelIndex, LevelDifficulities difficulity)
        {
            string levelName = GetLevelKey(stageIndex, levelIndex, difficulity);
            _levelData.TryGetValue(levelName, out BattleDataResponseDTO levelData);
            return levelData;
        }

        private static string GetLevelKey(int stage, int level)
        {
            return $"{stage}_{level}";
        }
        private static string GetLevelKey(int stage, int level, LevelDifficulities difficulity)
        {
            return $"{GetLevelKey(stage, level)}_{(int)difficulity}";
        }

    }
}
