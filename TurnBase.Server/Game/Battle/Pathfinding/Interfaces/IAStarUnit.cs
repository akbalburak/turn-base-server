using TurnBase.Server.Game.Battle.Interfaces.Battle;

namespace TurnBase.Server.Game.Battle.Pathfinding.Interfaces
{
    public interface IAStarUnit
    {
        IBattleUnitData UnitData { get; }

        public IAStarNode CurrentNode { get; }
        void ChangeNode(IAStarNode node);

        void OnAggrieving();
        void OnAggrieved();
    }
}
