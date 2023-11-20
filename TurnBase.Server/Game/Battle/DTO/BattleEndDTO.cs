using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Models;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleEndDTO
    {
        [JsonProperty("A")] public BattleEndSates BattleEndState { get; private set; }
        [JsonProperty("B")] public int WinnerTeam { get; set; }
        [JsonProperty("C")] public List<BattleRewardItemData> FirstCompletionRewards { get; set; }

        public BattleEndDTO(BattleEndSates battleEndState)
        {
            BattleEndState = battleEndState;
        }
    }
}
