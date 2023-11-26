namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleWave
    {
        public List<BattleNpcUnit> Units => _units;

        private List<BattleNpcUnit> _units;

        public MapData PathData => _pathData;

        public MapData _pathData;


        public BattleWave(List<BattleNpcUnit> units, MapData pathData)
        {
            _pathData = pathData;
            _units = units;
        }
    }

    public class MapData
    {
        public MapDataHexNodes[] MapHexNodes { get; private set; }
        public MapDataHexNodes[] PlayerSpawnPoints { get; set; }
        public MapData(MapDataHexNodes[] mapHexNodes,
            MapDataHexNodes[] playerSpawnPoints)
        {
            MapHexNodes = mapHexNodes;
            PlayerSpawnPoints = playerSpawnPoints;
        }
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
