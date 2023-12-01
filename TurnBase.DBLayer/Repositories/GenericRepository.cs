using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using System.Linq.Expressions;

namespace TurnBase.DBLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbTurnBaseDevContext _dbContext;

        public GenericRepository(DbTurnBaseDevContext context)
        {
            _dbContext = context;
        }

        public T Add(T entity)
        {
            return _dbContext.Set<T>().Add(entity).Entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public IQueryable<T> All()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public List<T> ToList()
        {
            return _dbContext.Set<T>().ToList();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Delete(int id)
        {
            T entity = _dbContext.Set<T>().Find(id);

            if (entity == null)
                return;

            _dbContext.Set<T>().Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
                _dbContext.Set<T>().Remove(entity);
        }

        public T Find(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            T inLocal = _dbContext.Set<T>().Local.FirstOrDefault(predicate.Compile());
            if (inLocal != null)
                return inLocal;
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public T Update(T entity)
        {
            EntityEntry<T> updatedItem = _dbContext.Set<T>().Update(entity);
            return updatedItem.Entity;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Any(predicate);
        }

        public EntityState GetStateOfEntry(T entity)
        {
            return _dbContext.Entry(entity).State;
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Count(predicate);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return _dbContext.Set<T>().Select(predicate);
        }

        public T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public IQueryable<T> Include(Expression<Func<T, object>> include)
        {
            return _dbContext.Set<T>().AsQueryable().Include(include);
        }
    }
}
