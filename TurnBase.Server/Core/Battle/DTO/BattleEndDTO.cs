using Newtonsoft.Json;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;

namespace TurnBase.Server.Core.Battle.DTO
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
