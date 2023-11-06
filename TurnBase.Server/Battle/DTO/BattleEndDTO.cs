using Newtonsoft.Json;
using TurnBase.Server.Battle.Enums;

namespace TurnBase.Server.Battle.DTO
{
    public class BattleEndDTO
    {
        [JsonProperty("A")] public BattleEndSates BattleEndState { get; private set; }
        [JsonProperty("B")] public int WinnerTeam { get; set; }
        public BattleEndDTO(BattleEndSates battleEndState)
        {
            this.BattleEndState = battleEndState;
        }
    }
}
