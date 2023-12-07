using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Serializables;

namespace TurnBase.Server.Game.Battle.Map
{
    public class MapDataEnemyDropJson : IMapDataEnemyDropJson
    {
        public float DropChance { get; set; }
        public int ItemId { get; set; }
        public int Level { get; set; }
        public float Quality { get; set; }
        public SerializableVector2Int Quantity { get; set; }

    }
}
