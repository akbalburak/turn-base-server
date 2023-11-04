using TurnBase.DBLayer.Models;

namespace TurnBase.DBLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Disposed { get; set; }
        IGenericRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
        DbTurnBaseDevContext GetContext { get; }
    }
}
