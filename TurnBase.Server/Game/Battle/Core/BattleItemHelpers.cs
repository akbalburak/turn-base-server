using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Game.Battle.Pathfinding.Interfaces;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public IBattleUser GetUser(ISocketUser socketUser)
        {
            return _users.FirstOrDefault(y => y.SocketUser == socketUser);
        }
        public IBattleUnit GetUnit(int targetUnitID)
        {
            return _allUnits.FirstOrDefault(y => y.UniqueId == targetUnitID);
        }
        public IBattleUnit GetAliveEnemyUnit(IBattleUnit owner)
        {
            return _allUnits.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.TeamIndex != owner.TeamIndex);
        }

        public IBattleUnit GetUnitInNode(int nodeIndex)
        {
            IAStarNode targetNode = this.GetNodeByIndex(nodeIndex);

            if (targetNode.OwnedBy is not IBattleUnit targetUnit)
                return null;

            // WE ARE LOOKING FOR THE TARGET.
            if (targetUnit.IsDeath)
                return null;

            return targetUnit;
        }

    }
}
