using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattlePath
    {
        int GetNodeIndex(IAStarNode node);
        int NodeSize { get; }
        IAStarNode GetNodeByIndex(int index);
        IAStarNode[] GetPath(IAStarNode fromPoint, IAStarNode toPoint);
    }
}
