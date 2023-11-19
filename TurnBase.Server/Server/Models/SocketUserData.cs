using TurnBase.DBLayer.Models;
using TurnBase.Server.Server.Interfaces;

namespace TurnBase.Server.Server.ServerModels
{
    public class SocketUserData : ISocketUserData
    {
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public Guid UserToken { get; private set; }
        public void AssignUser(TblUser user)
        {
            Id = user.Id;
            UserName = user.Username;
            UserToken = user.Token;
        }
    }
}
