using TurnBase.DBLayer.Models;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketUserData
    {
        string UserName { get; }
        long Id { get; }

        void AssignUser(TblUser user);
    }
}
