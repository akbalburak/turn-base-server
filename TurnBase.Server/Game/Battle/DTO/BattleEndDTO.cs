using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Map;
using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Game.DTO.Interfaces;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleEndDTO
    {
        [JsonProperty("A")] public BattleEndSates BattleEndState { get; private set; }
        [JsonProperty("B")] public int WinnerTeam { get; set; }
        [JsonProperty("C")] public BattleEndRewardDTO[] FirstCompletionRewards { get; set; }

        public BattleEndDTO(BattleEndSates battleEndState)
        {
            BattleEndState = battleEndState;
        }
    }

    public class BattleEndRewardDTO
    {
        [JsonProperty("A")] public int ItemID { get; set; }
        [JsonProperty("B")] public int Quantity { get; set; }
        [JsonProperty("C")] public int Level { get; set; }
        [JsonProperty("D")] public float Quality { get; set; }

        public BattleEndRewardDTO(IInventoryItemDTO reward)
        {
            ItemID = reward.ItemID;
            Quantity = reward.Quantity;
            Level = reward.Level;
            Quality = reward.Quality;
        }
    }
}
