using TurnBase.Server.Game.Battle.Interfaces;
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
        public IBattleUnit GetUnit(int uniqueId)
        {
            return _allUnits.FirstOrDefault(y => y.UnitData.UniqueId == uniqueId);
        }
        public IBattleUnit GetAliveEnemyUnit(IBattleUnit owner)
        {
            return _allUnits.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.UnitData.TeamIndex != owner.UnitData.TeamIndex);
        }

        public IBattleUnit GetAliveEnemyUnit(IBattleUnit owner, int distance)
        {
            IBattleUnit? aliveUnit = _allUnits
                .Where(x => !x.IsDeath)
                .Where(x => x.UnitData.TeamIndex != owner.UnitData.TeamIndex)
                .OrderBy(x => x.CurrentNode.GetDistance(owner.CurrentNode))
                .FirstOrDefault();

            if (aliveUnit == null)
                return null;

            float aliveDistance = aliveUnit.CurrentNode.GetDistance(owner.CurrentNode);
            if (aliveDistance > _difficulityData.MapData.DistancePerHex * distance)
                return null;

            return aliveUnit;
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
