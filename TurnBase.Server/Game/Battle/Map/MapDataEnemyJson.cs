using TurnBase.Server.Game.Battle.Map.Interfaces;

namespace TurnBase.Server.Game.Battle.Map
{
    public class MapDataEnemyJson : IMapDataEnemyJson
    {
        public int Group { get; set; }
        public int Enemy { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float TurnSpeed { get; set; }
        public int SpawnIndex { get; set; }
        public int AggroDistance { get; set; }
        public MapDataEnemyDropJson[] Drops { get; set; }

        public IMapDataEnemyDropJson[] IDrops => Drops;

        public MapDataEnemyJson()
        {
            Drops = Array.Empty<MapDataEnemyDropJson>();
        }
    }
}
