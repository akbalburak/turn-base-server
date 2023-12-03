using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleLevelData
    {
        public int Stage { get; set; }
        public int Level { get; set; }
        public List<BattleRewardItemData> FirstCompletionRewards { get; set; }
        public float DistancePerHex { get; set; }
        public MapDataHexNodes[] MapHexNodes { get; set; }
        public int[] PlayerSpawnPoints { get; set; }
        public MapDataEnemy[] Enemies { get; set; }

        public BattleLevelData()
        {
            MapHexNodes = Array.Empty<MapDataHexNodes>();
            Enemies = Array.Empty<MapDataEnemy>();
            FirstCompletionRewards = new List<BattleRewardItemData>();
            PlayerSpawnPoints = Array.Empty<int>();
        }
    }

    public class BattleRewardItemData
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public float Quality { get; set; }
    }

    public interface IMapDataEnemy
    {
        int Group { get; }
        int Enemy { get; }
        int Health { get; }
        int Damage { get; }
        float TurnSpeed { get; }
        int SpawnIndex { get; }
        int AggroDistance { get; }
    }

    public class MapDataEnemy : IMapDataEnemy
    {
        public int Group { get; set; }
        public int Enemy { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float TurnSpeed { get; set; }
        public int SpawnIndex { get; set; }
        public int AggroDistance { get; set; }
    }

    public class MapDataHexNodes
    {
        public SerializableVector3 Node { get; private set; }

        public MapDataHexNodes(SerializableVector3 node)
        {
            Node = node;
        }
    }

    [Serializable]
    public class SerializableVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
