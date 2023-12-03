using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleNpcUnitDTO : BattleUnitDTO
    {
        [JsonProperty("Z")] public int UnitId { get; set; }

        public BattleNpcUnitDTO(IBattleItem battleItem, IBattleNpcUnit battleNpcUnit)
            : base(battleItem, battleNpcUnit)
        {
            this.UnitId = battleNpcUnit.UnitId;
        }
    }
}
