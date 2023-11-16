using TurnBase.Server.Core.Battle.DTO;
using TurnBase.Server.Core.Battle.Enums;
using TurnBase.Server.Core.Battle.Models;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Core.Battle.Core
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
        public void SendToUser(BattleUser user, BattleActions battleAction, object data)
        {
            if (!user.IsConnected)
                return;

            SocketResponse dataToSend = BattleActionResponseDTO.GetSuccess(
                user.DataId,
                battleAction,
                data
            );

            user.SocketUser.AddToUnExpectedAfterSendIt(dataToSend);
        }

    }
}
