using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleItem
    {
        public Action<IBattleItem> OnDisposed { get; set; }

        public void ExecuteAction(SocketUser socketUser, BattleActionRequestDTO requestData);

        public void SendToAllUsers(BattleActions battleAction, object data);
        public void SendToUser(BattleUser user, BattleActions battleAction, object data);

        public BattleUser GetUser(SocketUser socketUser);
        public BattleUnit GetUnit(int targetUnitID);
        public BattleUnit GetAliveEnemyUnit(BattleUnit owner);

        public void EndTurn();
    }
}
