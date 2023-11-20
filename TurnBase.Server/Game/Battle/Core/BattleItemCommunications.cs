using TurnBase.Server.Game.Battle.DTO;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Battle.Interfaces;
using TurnBase.Server.Game.Battle.Models;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        public void SendToAllUsers(BattleActions battleAction, object data)
        {
            foreach (BattleUser user in _users)
            {
                SendToUser(user, battleAction, data);
            }
        }
        public void SendToUser(IBattleUser user, BattleActions battleAction, object data)
        {
            if (!user.IsConnected)
                return;

            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                user.GetNewDataId,
                battleAction,
                data
            );

            user.SocketUser.SendToClient(dataToSend);
        }

    }
}
