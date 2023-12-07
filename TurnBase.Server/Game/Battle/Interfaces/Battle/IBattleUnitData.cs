using TurnBase.Server.Game.Battle.Map.Interfaces;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleUnitData
    {
        int TeamIndex { get; }
        int GroupIndex { get; }
        int UniqueId { get; }
        int AggroDistance { get; }
        IBattleItem BattleItem { get; }
        IAStarNode InitialNode { get; }

        IMapDataEnemyDropJson[] IDrops { get; }
    }
}
