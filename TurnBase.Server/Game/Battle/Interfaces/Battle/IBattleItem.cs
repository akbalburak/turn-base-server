using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleItem
    {
        public double GetRandomValue { get; }
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
