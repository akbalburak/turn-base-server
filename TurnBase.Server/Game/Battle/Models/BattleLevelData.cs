﻿using TurnBase.Server.Game.Enums;

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
        public List<BattleWave> Waves { get; private set; }
        public List<BattleRewardItemData> FirstCompletionRewards { get; set; }
        public BattleDifficulityData()
        {
            Waves = new List<BattleWave>();
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
}