using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Interfaces
{
    public interface IBattleItem
    {
        public Action<IBattleItem> OnDisposed { get; set; }

        public void ExecuteAction(ISocketUser socketUser, BattleActionRequestDTO requestData);

        public void SendToAllUsers(BattleActions battleAction, object data);
        public void SendToUser(IBattleUser user, BattleActions battleAction, object data);

        public IBattleUser GetUser(ISocketUser socketUser);
        public IBattleUnit GetUnit(int targetUnitID);
        public IBattleUnit GetAliveEnemyUnit(IBattleUnit owner);

        public void FinalizeTurn();
    }
}
