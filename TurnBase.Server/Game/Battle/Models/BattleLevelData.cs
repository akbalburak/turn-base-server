using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleLevelData
    {
        public int Stage { get; private set; }
        public int Level { get; set; }
        public List<BattleDifficulityData> Difficulities { get; private set; }

        public BattleLevelData(int stage, int level, List<BattleDifficulityData> difficulities)
        {
            Stage = stage;
            Level = level;
            Difficulities = difficulities;
        }

        public BattleDifficulityData GetDifficulityData(LevelDifficulities difficulity)
        {
            if (Difficulities.Count == 0)
                return null;

            BattleDifficulityData difficulityData = Difficulities.FirstOrDefault(y => y.Difficulity == difficulity);
            if (difficulityData != null)
                return difficulityData;

            return null;
        }
    }

    public class BattleDifficulityData
    {
        public LevelDifficulities Difficulity { get; set; }
        public MapData MapData { get; set; }
        public List<BattleRewardItemData> FirstCompletionRewards { get; set; }
        public BattleDifficulityData()
        {
            FirstCompletionRewards = new List<BattleRewardItemData>();
        }
    }

    public class BattleRewardItemData
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public float Quality { get; set; }
    }

    [Serializable]
    public class SerializableVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
