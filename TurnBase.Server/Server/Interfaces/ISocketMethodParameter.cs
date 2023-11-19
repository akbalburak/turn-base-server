using TurnBase.DBLayer.Interfaces;

namespace TurnBase.Server.Server.Interfaces
{
    public interface ISocketMethodParameter : IDisposable, IChangeHandler
    {
        IUnitOfWork UOW { get; }
        ISocketUser SocketUser { get; }

        void ExecuteOnSuccess();
        T GetRequestData<T>();
    }
}
