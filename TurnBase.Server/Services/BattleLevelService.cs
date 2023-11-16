using TurnBase.Server.Battle.Models;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.Services
{
    public static class BattleLevelService
    {
        private static Dictionary<string, string> _levels
            = new Dictionary<string, string>();

        private static Dictionary<string, BattleLevelData> _levelMetaData
            = new Dictionary<string, BattleLevelData>();

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

                _levelMetaData.Add(levelData.Key, levelData);
                _levels.Add(levelData.Key, fileData);
            }
        }

        public static BattleLevelData GetLevelData(int stageIndex,int levelIndex)
        {
            string levelName = $"Level_{stageIndex}_{levelIndex}";

            if (!_levels.TryGetValue(levelName, out string levelData))
                return null;

            return levelData.ToObject<BattleLevelData>();
        }

        public static BattleLevelData GetLevelMetaData(int stageIndex, int levelIndex)
        {
            string levelName = $"Level_{stageIndex}_{levelIndex}";
            _levelMetaData.TryGetValue(levelName, out BattleLevelData levelData);
            return levelData;
        }

    }
}
