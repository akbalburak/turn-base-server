using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Pathfinding.Core;

namespace TurnBase.Server.Game.Battle.Pathfinding.Interfaces
{
    public interface IAStarNode
    {
        Action<IAStarUnit> AggroUnits { get; set; }
        void TriggerAggro(IAStarUnit owner);

        float X { get; }
        float Z { get; }

        float GetDistance(IAStarNode node);
        void FindNeighbors(IAStarNode[] nodes, float distancePerHex);
        IAStarNode[] GetInDistance(int distance);

        IAStarNode[] Neighbors { get; }

        int Cost { get; set; }
        int Heuristic { get; set; }
        IAStarNode Parent { get; set; }

        IAStarUnit OwnedBy { get; }
        void SetOwner(IAStarUnit owner);
    }
}
