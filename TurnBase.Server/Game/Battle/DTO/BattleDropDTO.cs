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
        [JsonProperty("A")] public int DropItemId { get; }
        [JsonProperty("B")] public int ItemId { get; }
        [JsonProperty("C")] public int Level { get; }
        [JsonProperty("D")] public float Quality { get; }
        [JsonProperty("E")] public bool Claimed { get; }


        public BattleDropItemDTO(IBattleDropItem dropData)
        {
            DropItemId = dropData.DropItemId;
            ItemId = dropData.ItemId;
            Level = dropData.Level;
            Quality = dropData.Quality;
            Claimed = dropData.Claimed;
        }
    }

    public class BattleDropClaimRequestDTO
    {
        [JsonProperty("A")] public int UnitUniqueId { get; set; }
        [JsonProperty("B")] public int DropItemId { get; set; }

        public const int All = -1;
    }

    public class BattleDropClaimResponseDTO
    {
        [JsonProperty("A")] public int DropUnitId { get; }
        [JsonProperty("B")] public int[] ClaimedDropItemIds { get; }

        public BattleDropClaimResponseDTO(int dropUnitId, IEnumerable<int> ids)
        {
            DropUnitId = dropUnitId;
            ClaimedDropItemIds = ids.ToArray();
        }
    }
}
