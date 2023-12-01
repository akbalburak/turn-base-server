using TurnBase.Server.Game.Battle.Pathfinding.Core;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        private AStarNode[] _nodes;

        public int NodeSize => _nodes.Length;

        public IAStarNode GetNodeByIndex(int index)
        {
            return _nodes[index];
        }
        public int GetNodeIndex(IAStarNode node)
        {
            return Array.IndexOf(_nodes, node);
        }

        public IAStarNode[] GetPath(IAStarNode fromPoint, IAStarNode toPoint)
        {
            // WE LOOK FOR THE PATH.
            return AStar.FindPath(_nodes, fromPoint, toPoint);
        }
    }
}
