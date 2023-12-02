using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Pathfinding.Core
{
    public class AStarNode : IAStarNode
    {
        public Action<IAStarUnit> AggroUnits { get; set; }

        public float X { get; set; }
        public float Z { get; set; }
        public int Cost { get; set; }
        public int Heuristic { get; set; }
        public IAStarNode Parent { get; set; }

        public IAStarNode[] Neighbors { get; set; }

        public IAStarUnit OwnedBy { get; set; }

        public AStarNode(float x, float z)
        {
            Neighbors = Array.Empty<IAStarNode>();
            X = x;
            Z = z;
        }

        public float GetDistance(IAStarNode node)
        {
            float num = X - node.X;
            float num2 = Z - node.Z;
            return (float)Math.Sqrt(num * num + num2 * num2);
        }

        public void FindNeighbors(IAStarNode[] nodes, float maxDistance)
        {
            List<IAStarNode> neighbors = new List<IAStarNode>();

            foreach (IAStarNode node in nodes)
            {
                if (node == this)
                    continue;

                float distance = GetDistance(node);
                if (distance <= maxDistance)
                {
                    neighbors.Add(node);
                }
            }

            Neighbors = neighbors.ToArray();
        }
        public IAStarNode[] GetInDistance(int distance)
        {
            if (distance <= 0)
                return Array.Empty<IAStarNode>();

            List<IAStarNode> nodes = new List<IAStarNode>();

            IAStarNode[] neighbors = new IAStarNode[] { this };

            while (distance > 0)
            {
                distance--;

                neighbors = neighbors
                    .SelectMany(y => y.Neighbors)
                    .Distinct()
                    .Where(x => !nodes.Contains(x))
                    .ToArray();

                nodes.AddRange(neighbors);
            }

            return nodes.ToArray();
        }

        public void SetOwner(IAStarUnit owner)
        {
            // WE REMOVE OLDER AGGROS.
            if (OwnedBy != null)
            {
                IAStarNode[] nodes = GetInDistance(OwnedBy.UnitData.AggroDistance);
                foreach (IAStarNode node in nodes)
                {
                    node.AggroUnits -= OwnedBy.OnAggrieving;
                }
            }

            OwnedBy = owner;

            if (OwnedBy != null)
            {
                IAStarNode[] nodes = GetInDistance(OwnedBy.UnitData.AggroDistance);
                foreach (IAStarNode node in nodes)
                {
                    node.AggroUnits += OwnedBy.OnAggrieving;
                }
            }
        }
        public void TriggerAggro(IAStarUnit unit)
        {
            AggroUnits?.Invoke(unit);
        }
    }
}
