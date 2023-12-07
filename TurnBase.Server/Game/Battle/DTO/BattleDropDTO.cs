using Newtonsoft.Json;
using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.DTO
{
    public class BattleDropDTO
    {
        [JsonProperty("A")] public int DropUnitId { get; }
        [JsonProperty("B")] public BattleDropItemDTO[] Drops { get; }
        public BattleDropDTO(IBattleDrop dropData)
        {
            DropUnitId = dropData.KilledUnit.UnitData.UniqueId;
            Drops = dropData.Drops.Select(y => new BattleDropItemDTO(y)).ToArray();
        }
    }

    public class BattleDropItemDTO
    {
        [JsonProperty("A")] public int ItemId { get; }
        [JsonProperty("B")] public int Level { get; }
        [JsonProperty("C")] public float Quality { get; }
        [JsonProperty("D")] public bool Claimed { get; }

        public BattleDropItemDTO(IBattleDropItem dropData)
        {
            ItemId = dropData.ItemId;
            Level = dropData.Level;
            Quality = dropData.Quality;
            Claimed = dropData.Claimed;
        }
    }
}
