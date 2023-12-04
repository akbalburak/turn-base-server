using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Core;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleTurnDTO
    {
        [JsonProperty("A")] public int UnitId { get; set; }
        public BattleTurnDTO(int unitId)
        {
            UnitId = unitId;
        }
    }

    public class BattleTurnChangedDTO
    {
        [JsonProperty("A")] public BattleTurnChangedItemDTO[] TurnData { get; }
        public BattleTurnChangedDTO(IBattleTurnHandler turnHandler)
        {
            TurnData = turnHandler.TurnItems
                .Select(y => new BattleTurnChangedItemDTO(y))
                .ToArray();
        }
    }
    public class BattleTurnChangedItemDTO
    {
        [JsonProperty("A")] public float BaseAttackTurn { get; }
        [JsonProperty("B")] public float NextAttackTurn { get; }
        [JsonProperty("C")] public int UnitId { get; }
        public BattleTurnChangedItemDTO(BattleTurnHandler.IBattleTurnItem turnItem)
        {
            UnitId = turnItem.Unit.UnitData.UniqueId;
            BaseAttackTurn = turnItem.BaseAttackTurn;
            NextAttackTurn = turnItem.NextAttackTurn;
        }

    }
}
