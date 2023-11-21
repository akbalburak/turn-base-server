using Newtonsoft.Json;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Game.Trackables
{
    public abstract class TrackableDTO : IChangeItem
    {
        private IChangeHandler _changeHandler;
        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            _changeHandler = changeHandler;
        }

        public void SetAsModified()
        {
            _changeHandler.AddChanges(this);
        }

        public abstract SocketResponse GetResponse();
    }
}
