namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleWave
    {
        public MapData PathData => _pathData;

        public MapData _pathData;

        public BattleWave(MapData pathData)
        {
            _pathData = pathData;
        }
    }

    public class MapData
    {
        public MapDataHexNodes[] MapHexNodes { get; set; }
        public int[] PlayerSpawnPoints { get; set; }
        public float DistancePerHex { get; set; }
        public MapDataEnemy[] Enemies { get; set; }

        public MapData()
        {
            MapHexNodes = Array.Empty<MapDataHexNodes>();
            Enemies = Array.Empty<MapDataEnemy>();
        }
    }

    public interface IMapDataEnemy
    {
        int Enemy { get; }
        int Health { get; }
        int Damage { get; }
        float TurnSpeed { get; }
        int SpawnIndex { get; set; }
    }

    public class MapDataEnemy : IMapDataEnemy
    {
        public int Enemy { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float TurnSpeed { get; set; }
        public int SpawnIndex { get; set; }
    }

    public class MapDataHexNodes
    {
        public SerializableVector3 Node { get; private set; }

        public MapDataHexNodes(SerializableVector3 node)
        {
            Node = node;
        }
    }
}
