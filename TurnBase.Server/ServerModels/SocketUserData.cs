using TurnBase.DBLayer.Models;

namespace TurnBase.Server.ServerModels
{
    public class SocketUserData
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
