using TurnBase.Server.Battle.Models;
using TurnBase.Server.ServerModels;

namespace TurnBase.Server.Battle
{
    public class BattleUser : BattleUnitAttack
    {
        public bool IsReady { get; private set; }
        public SocketUser SocketUser { get; private set; }
        public string PlayerName { get; private set; }
        public bool IsConnected => SocketUser != null;

        public BattleUser(SocketUser user,
            string playerName,
            int health,
            int position,
            int minDamage,
            int maxDamage,
            float attackSpeed)
            : base(health, position, minDamage, maxDamage, attackSpeed)
        {
            this.SocketUser = user;
            this.PlayerName = playerName;
        }

        public void SetAsReady()
        {
            IsReady = true;
        }
    }
}
