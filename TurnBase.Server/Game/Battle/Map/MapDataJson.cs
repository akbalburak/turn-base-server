using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Game.Battle.Map.Json;

namespace TurnBase.Server.Game.Battle.Map
{
    public class MapDataJson : IMapDataJson
    {
        public int Stage { get; set; }
        public int Level { get; set; }
        public float DistancePerHex { get; set; }
        public int[] PlayerSpawnPoints { get; set; }
        public MapDataNodeJson[] MapHexNodes { get; set; }
        public MapDataEnemyJson[] Enemies { get; set; }
        public MapDataFirstTimeRewardJson[] FirstCompletionRewards { get; set; }

        public int[] IPlayerSpawnPoints => PlayerSpawnPoints.ToArray();
        public IMapDataEnemyJson[] IEnemies => Enemies.ToArray();
        public IMapDataFirstTimeRewardJson[] IFirstCompletionRewards => FirstCompletionRewards.ToArray();
        public IMapDataNodeJson[] IMapHexNodes => MapHexNodes.ToArray();


        public MapDataJson()
        {
            MapHexNodes = Array.Empty<MapDataNodeJson>();
            Enemies = Array.Empty<MapDataEnemyJson>();
            FirstCompletionRewards = Array.Empty<MapDataFirstTimeRewardJson>();
            PlayerSpawnPoints = Array.Empty<int>();
        }
    }
}
