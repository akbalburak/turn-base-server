namespace TurnBase.Server.Game.Battle.Pathfinding
{
    public class AStarNode : IAstarNode
    {
        public float X { get; set; }
        public float Z { get; set; }
        public int Cost { get; set; }
        public int Heuristic { get; set; }
        public AStarNode Parent { get; set; }

        public IAstarNode[] Neighbors { get; set; }

        public AStarNode(float x, float z)
        {
            Neighbors = Array.Empty<IAstarNode>();
            X = x;
            Z = z;
        }

        public float GetDistance(IAstarNode node)
        {
            float num = X - node.X;
            float num2 = Z - node.Z;
            return (float)Math.Sqrt(num * num + num2 * num2);
        }

        public void FindNeighbors(AStarNode[] nodes, float maxDistance)
        {
            List<AStarNode> neighbors = new List<AStarNode>();

            foreach (AStarNode node in nodes)
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
    }
}
