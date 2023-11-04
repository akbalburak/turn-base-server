using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;

namespace TurnBase.DBLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbTurnBaseDevContext _context;
        public UnitOfWork()
        {
            _context = new DbTurnBaseDevContext();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public int SaveChanges() => _context.SaveChanges();

        public DbTurnBaseDevContext GetContext => _context;

        #region Disposable

        public bool Disposed { get; set; }
        public void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
