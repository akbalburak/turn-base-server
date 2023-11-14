using TurnBase.Server.Battle.Models;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.Battle.Services
{
    public static class BattleLevelService
    {
        private static Dictionary<string, string> _levels
            = new Dictionary<string, string>();

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

                _levels.Add(levelData.Key, fileData);
            }
        }

        public static BattleLevelData GetLevelData(string levelName)
        {
            if (!_levels.TryGetValue(levelName, out string levelData))
                return null;

            return levelData.ToObject<BattleLevelData>();
        }

    }
}
