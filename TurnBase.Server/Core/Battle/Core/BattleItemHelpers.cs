using TurnBase.Server.Core.Battle.Interfaces;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Core
{
    public partial class BattleItem
    {
        public BattleUser GetUser(SocketUser socketUser)
        {
            return _users.FirstOrDefault(y => y.SocketUser == socketUser);
        }
        public BattleUnit GetUnit(int targetUnitID)
        {
            BattleUser? user = _users.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (user != null)
                return user;

            BattleNpcUnit unit = _currentWave.Units.FirstOrDefault(y => y.UniqueId == targetUnitID);
            if (unit != null)
                return unit;

            return null;
        }
        public BattleUnit GetAliveEnemyUnit(BattleUnit owner)
        {
            return _currentWave.Units.OrderBy(y => Guid.NewGuid())
                .FirstOrDefault(y => !y.IsDeath && y.TeamIndex != owner.TeamIndex);
        }

    }
}
