using TurnBase.Server.Serializables;

namespace TurnBase.Server.Game.Battle.Map.Interfaces
{
    public interface IMapDataEnemyDropJson
    {
        public float DropChance { get; }
        public int ItemId { get; }
        public int Level { get; }
        public float Quality { get; }
        public SerializableVector2Int Quantity { get; }
    }
}
