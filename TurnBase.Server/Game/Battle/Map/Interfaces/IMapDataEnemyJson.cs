namespace TurnBase.Server.Game.Battle.Map.Interfaces
{
    public interface IMapDataEnemyJson
    {
        int Group { get; }
        int Enemy { get; }
        int Health { get; }
        int Damage { get; }
        float TurnSpeed { get; }
        int SpawnIndex { get; }
        int AggroDistance { get; }
        IMapDataEnemyDropJson[] IDrops { get; }
    }
}
