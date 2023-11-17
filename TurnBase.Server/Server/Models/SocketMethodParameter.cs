using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Extends.Json;
using TurnBase.Server.Interfaces;

namespace TurnBase.Server.Server.ServerModels
{
    public class SocketMethodParameter : IDisposable, IChangeManager
    {
        public DateTime RequestDate { get; }

        public IUnitOfWork UOW { get; set; }

        public SocketUser SocketUser { get; set; }

        public SocketRequest Request { get; set; }

        public bool IsDisposed { get; private set; }

        private readonly List<IChangeItem> _changes;

        public SocketMethodParameter(SocketUser socketUser, SocketRequest request)
        {
            _changes = new List<IChangeItem>();
            RequestDate = DateTime.UtcNow;
            SocketUser = socketUser;
            Request = request;
            UOW = new UnitOfWork();
        }


        public T GetRequestData<T>()
        {
            if (Request.Data == null)
                return (T)Activator.CreateInstance(typeof(T));
            return Request.Data.ToObject<T>();
        }

        public void ExecuteOnSuccess()
        {
            SendAllChanges();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (!UOW.Disposed)
                UOW.Dispose();
        }

        
        public void AddChanges(IChangeItem changeData)
        {
            _changes.Add(changeData);
        }
        private void SendAllChanges()
        {
            if (_changes.Count == 0)
                return;

            _changes.ForEach(item =>
            {
                SocketResponse responseData = item.GetResponse();
                SocketUser.AddToUnExpectedAfterSendIt(responseData);
            });
        }
    }
}
