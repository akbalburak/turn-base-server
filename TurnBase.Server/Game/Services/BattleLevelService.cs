using TurnBase.Server.Extends.Json;
using static TurnBase.Server.Game.DTO.BattleDTO;
using TurnBase.Server.Game.Battle.Map;

namespace TurnBase.Server.Game.Services
{
    public static class BattleLevelService
    {
        private static Dictionary<string, string> _levels
            = new Dictionary<string, string>();

        private static Dictionary<string, BattleDataResponseDTO> _levelData
            = new Dictionary<string, BattleDataResponseDTO>();

        public static void Initialize()
        {
            string[] files = Directory.GetFiles("Data/Stages", string.Empty, SearchOption.AllDirectories);

            foreach (string file in files)
            {
                if (!file.EndsWith(".json"))
                    continue;

                string fileData = File.ReadAllText(file);

                MapDataJson levelData = fileData.ToObject<MapDataJson>();
                if (levelData == null)
                    continue;

                // WE ASSIGN ALL DIFFICULITIES WITH THEIR LEVELS.
                string levelName = GetLevelKey(levelData.Stage, levelData.Level);
                _levelData.Add(levelName, new BattleDataResponseDTO
                {
                    FirstTimeRewards = levelData.FirstCompletionRewards.Select(y => new BattleDataRewardItemDTO
                    {
                        ItemId = y.ItemID,
                        Level = y.Level,
                        Quality = y.Quality,
                        Quantity = y.Quantity,
                    }).ToList()
                });

                _levels.Add(GetLevelKey(levelData.Stage, levelData.Level), fileData);
            }
        }

        public static MapDataJson GetLevelData(int stageIndex, int levelIndex)
        {
            string levelName = GetLevelKey(stageIndex, levelIndex);

            if (!_levels.TryGetValue(levelName, out string levelData))
                throw new Exception("Level Not Found");

            return levelData.ToObject<MapDataJson>();
        }

        public static BattleDataResponseDTO GetLevelMetaData(int stageIndex, int levelIndex)
        {
            string levelName = GetLevelKey(stageIndex, levelIndex);
            _levelData.TryGetValue(levelName, out BattleDataResponseDTO levelData);
            return levelData;
        }

        private static string GetLevelKey(int stage, int level)
        {
            return $"{stage}_{level}";
        }
    }
}
