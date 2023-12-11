using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleCombatStateChangedDTO
    {
        [JsonProperty("A")] public bool IsInCombat { get; }
        public BattleCombatStateChangedDTO(IBattleTurnHandler battleTurnHandler)
        {
            IsInCombat = battleTurnHandler.IsInCombat;    
        }
    }
}
