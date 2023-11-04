using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Extends.Json;

namespace TurnBase.Server.ServerModels
{
    public class SocketMethodParameter : IDisposable
    {
        #region PROPS

        public DateTime RequestDate { get; }

        public IUnitOfWork UOW { get; set; }

        public SocketUser SocketUser { get; set; }

        public SocketRequest Request { get; set; }

        public bool IsDisposed { get; private set; }

        public List<Tuple<SocketUser, SocketResponse>> WaitingUnExpectedActions { get; set; }

        #endregion

        #region CONSTRUCTOR

        public SocketMethodParameter(SocketUser socketUser, SocketRequest request)
        {
            RequestDate = DateTime.UtcNow;
            this.SocketUser = socketUser;
            this.Request = request;
            UOW = new UnitOfWork();
            WaitingUnExpectedActions = new List<Tuple<SocketUser, SocketResponse>>();
        }

        public SocketMethodParameter(IUnitOfWork uow, DateTime actionDate)
        {
            this.UOW = uow;
            this.RequestDate = actionDate;
            WaitingUnExpectedActions = new List<Tuple<SocketUser, SocketResponse>>();
        }

        public SocketMethodParameter(DateTime actionDate)
        {
            this.UOW = new UnitOfWork();
            this.RequestDate = actionDate;
            WaitingUnExpectedActions = new List<Tuple<SocketUser, SocketResponse>>();
        }

        #endregion

        #region USER BIND

        public void LoadUserData()
        {
        }

        #endregion

        #region DATA CONTROLL

        public T GetRequestData<T>()
        {
            if (Request.Data == null)
                return (T)Activator.CreateInstance(typeof(T));
            return Request.Data.ToObject<T>();
        }

        #endregion

        #region UNEXPECTED QUEUEU

        public void AddToUnExpectedQueue(int userId, SocketResponse data)
        {
            SocketUser socketUser = null;
            if (socketUser == null)
                return;

            lock (WaitingUnExpectedActions)
                WaitingUnExpectedActions.Add(new Tuple<SocketUser, SocketResponse>(socketUser, data));
        }

        public void SendUsersToUnExpectedQueue()
        {
            lock (WaitingUnExpectedActions)
            {
                WaitingUnExpectedActions.ForEach(e => e.Item1.AddToUnExpectedAfterSendIt(e.Item2));
                WaitingUnExpectedActions.Clear();
            }
        }

        public void ExecuteOnSuccess()
        {
        }

        #endregion

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (!UOW.Disposed)
                UOW.Dispose();
        }
    }
}
