using TurnBase.DBLayer.Models;
using TurnBase.Server.Battle.Models;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle
{
    public class BattleUser : BattleUnit
    {
        public bool IsReady { get; private set; }
        public SocketUser SocketUser { get; private set; }
        public string PlayerName { get; private set; }
        public bool IsFirstCompletion { get; private set; }

        public bool IsConnected => SocketUser != null;

        private int _dataId;
        public int DataId => ++_dataId;
        public BattleUser(SocketUser socketUser, string playerName, int position, UnitStats stats, bool isFirstCompletion)
            : base(position, stats)
        {
            this.SocketUser = socketUser;
            this.PlayerName = playerName;
            this.IsFirstCompletion = isFirstCompletion;
        }

        public void SetAsReady()
        {
            IsReady = true;
        }
    }
}
