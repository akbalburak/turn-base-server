using TurnBase.Server.Extends.Json;
using TurnBase.Server.Game.Battle.Map;

namespace TurnBase.Server.Game.Services
{
    public static class BattleLevelService
    {
        private static Dictionary<string, string> _levels
            = new Dictionary<string, string>();

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

                _levels.Add(levelName, fileData);
            }
        }

        public static MapDataJson GetLevelData(int stageIndex, int levelIndex)
        {
            string levelName = GetLevelKey(stageIndex, levelIndex);

            if (!_levels.TryGetValue(levelName, out string levelData))
                throw new Exception("Level Not Found");

            return levelData.ToObject<MapDataJson>();
        }

        private static string GetLevelKey(int stage, int level)
        {
            return $"{stage}_{level}";
        }
    }
}
