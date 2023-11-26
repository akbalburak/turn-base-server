using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Models;
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
            IBattleUser? user = _users.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (user != null)
                return user;

            IBattleUnit unit = _allUnits.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (unit != null)
                return unit;

            return null;
        }
        public IBattleUnit GetAliveEnemyUnit(IBattleUnit owner)
        {
            return _allUnits.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.TeamIndex != owner.TeamIndex);
        }

    }
}
