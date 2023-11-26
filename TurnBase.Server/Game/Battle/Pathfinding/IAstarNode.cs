namespace TurnBase.Server.Game.Battle.Pathfinding
{
    public interface IAstarNode
    {
        float X { get; }
        float Z { get; }

        float GetDistance(IAstarNode node);

        IAstarNode[] Neighbors { get; }

    }
}
