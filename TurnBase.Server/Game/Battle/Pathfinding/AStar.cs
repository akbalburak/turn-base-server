namespace TurnBase.Server.Game.Battle.Pathfinding
{
    public static class AStar
    {
        public static AStarNode[] FindPath(AStarNode[] nodes, AStarNode start, AStarNode goal)
        {
            List<AStarNode> openList = new List<AStarNode>();
            List<AStarNode> closedList = new List<AStarNode>();

            openList.Add(start);

            // WE REMOVE OLD NODE PARENTS.
            foreach (AStarNode node in nodes)
                node.Parent = null;

            while (openList.Count > 0)
            {
                AStarNode current = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].Cost + openList[i].Heuristic < current.Cost + current.Heuristic)
                    {
                        current = openList[i];
                    }
                }

                openList.Remove(current);
                closedList.Add(current);

                if (current.X == goal.X && current.Z == goal.Z)
                {
                    // PATH FOUND, RECONSTRUCT AND RETURN IT
                    List<AStarNode> path = new List<AStarNode>();
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Parent;
                    }
                    path.Reverse();
                    return path.ToArray();
                }

                foreach (AStarNode neighbor in current.Neighbors)
                {
                    if (closedList.Contains(neighbor))
                        continue;

                    int tentativeCost = current.Cost + 1;

                    if (!openList.Contains(neighbor) || tentativeCost < neighbor.Cost)
                    {
                        neighbor.Parent = current;
                        neighbor.Cost = tentativeCost;
                        neighbor.Heuristic = CalculateHeuristic(neighbor, goal);

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }

            // NO PATH FOUND
            return Array.Empty<AStarNode>();
        }

        private static bool IsValidCell(float x, float y, AStarNode[] grid)
        {
            return true;//x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);//&& grid[x, y] == 0;
        }

        private static int CalculateHeuristic(AStarNode current, AStarNode goal)
        {
            return (int)(Math.Abs(current.X - goal.X) + Math.Abs(current.Z - goal.Z));
        }
    }
}
