﻿
using Newtonsoft.Json;
using TurnBase.DTOLayer.Enums;

namespace TurnBase.DTOLayer.Models
{
    public class BattleDTO
    {
        public class BattleRequestDTO
        {
            [JsonProperty("A")] public int StageIndex { get; set; }
            [JsonProperty("B")] public int LevelIndex { get; set; }
            [JsonProperty("C")] public LevelDifficulities Difficulity { get; set; }
        }

        public class BattleDataRequestDTO
        {
            [JsonProperty("A")] public int StageIndex { get; set; }
            [JsonProperty("B")] public int LevelIndex { get; set; }
            [JsonProperty("C")] public LevelDifficulities Difficulity { get; set; }
        }

        public class BattleDataResponseDTO
        {
            [JsonProperty("A")] public List<BattleRewardItemData> FirstTimeRewards { get; set; }
        }

        public class BattleRewardItemData
        {
            [JsonProperty("A")] public int ItemId { get; set; }
            [JsonProperty("B")] public int Quantity { get; set; }
            [JsonProperty("C")] public int Level { get; set; }
            [JsonProperty("D")] public float Quality { get; set; }
        }
    }
}
