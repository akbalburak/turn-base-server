namespace TurnBase.Server.Game.Battle.Map.Interfaces
{
    public interface IMapDataJson
    {
        IMapDataNodeJson[] IMapHexNodes { get; }
        IMapDataEnemyJson[] IEnemies { get; }
        IMapDataFirstTimeRewardJson[] IFirstCompletionRewards { get; }
        float DistancePerHex { get; }
        int[] IPlayerSpawnPoints { get; }
        int Stage { get; }
        int Level { get; }
    }
}
