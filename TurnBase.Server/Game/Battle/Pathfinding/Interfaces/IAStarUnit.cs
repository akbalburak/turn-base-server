namespace TurnBase.Server.Game.Battle.Pathfinding.Interfaces
{
    public interface IAStarUnit
    {
        public IAStarNode CurrentNode { get; }
        void ChangeNode(IAStarNode node);
    }
}
