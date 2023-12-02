using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Pathfinding.Core
{
    public static class AStar
    {
        public static IAStarNode[] FindPath(IAStarNode[] nodes, IAStarNode start, IAStarNode goal)
        {
            List<IAStarNode> openList = new List<IAStarNode>();
            List<IAStarNode> closedList = new List<IAStarNode>();

            openList.Add(start);

            // WE REMOVE OLD NODE PARENTS.
            foreach (IAStarNode node in nodes)
            {
                node.Parent = null;
                node.Cost = 0;
                node.Heuristic = 0;
            }

            while (openList.Count > 0)
            {
                IAStarNode current = openList[0];

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
                    List<IAStarNode> path = new List<IAStarNode>();
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Parent;
                    }

                    path.Reverse();

                    // IF THE LAST POINT IS INVALID WE WILL GO PREVIOUS ONE.
                    if (!IsValidCell(goal))
                        path.Remove(goal);

                    return path.ToArray();
                }

                foreach (IAStarNode neighbor in current.Neighbors)
                {
                    // IF NEIGHBOR ITSELF WE WONT CHECK IF THE CELL IS VALID HERE.
                    if (neighbor != goal)
                    {
                        if (!IsValidCell(neighbor))
                            continue;
                    }

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
            return Array.Empty<IAStarNode>();
        }

        private static bool IsValidCell(IAStarNode node)
        {
            return node.OwnedBy == null;
        }

        private static int CalculateHeuristic(IAStarNode current, IAStarNode goal)
        {
            return (int)(Math.Abs(current.X - goal.X) + Math.Abs(current.Z - goal.Z));
        }

    }
}
