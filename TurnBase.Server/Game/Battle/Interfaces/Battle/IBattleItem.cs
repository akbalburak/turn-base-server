using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleItem : IBattlePath
    {
        bool IsInCombat { get; }
        void CallGroupAggrieving(int group);

        double GetRandomValue { get; }
        Action<IBattleItem> OnDisposed { get; set; }

        void ExecuteAction(ISocketUser socketUser, BattleActionRequestDTO requestData);

        void SendToAllUsers(BattleActions battleAction, object data);
        void SendToUser(IBattleUser user, BattleActions battleAction, object data);

        IBattleUser GetUser(ISocketUser socketUser);
        IBattleUnit GetUnit(int targetUnitID);
        IBattleUnit GetAliveEnemyUnit(IBattleUnit owner);
        IBattleUnit GetUnitInNode(int nodeIndex);

        void FinalizeTurn();
    }
}
