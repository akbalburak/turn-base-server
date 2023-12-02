using TurnBase.Server.Game.Battle.Interfaces.Battle;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;

namespace TurnBase.Server.Game.Battle.Models
{
    public class BattleUnitData : IBattleUnitData
    {
        public int UniqueId { get; }
        public IBattleItem BattleItem { get; }
        public int TeamIndex { get; }
        public int GroupIndex { get; }
        public IAStarNode InitialNode { get; }
        public int AggroDistance { get; }

        public BattleUnitData(int uniqueId,
                              IBattleItem battleItem,
                              int teamIndex,
                              int groupIndex,
                              IAStarNode initialNode,
                              int aggroDistance)
        {
            AggroDistance = aggroDistance;
            UniqueId = uniqueId;
            BattleItem = battleItem;
            TeamIndex = teamIndex;
            GroupIndex = groupIndex;
            InitialNode = initialNode;
        }
    }
}
