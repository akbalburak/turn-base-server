using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TurnBase.DBLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        List<T> ToList();
        IQueryable<T> All();
        T Find(int id);
        bool Any(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate);
        T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate);
        T Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        void Delete(IEnumerable<T> entities);
        T Add(T entity);
        EntityState GetStateOfEntry(T entity);
        int Count(Expression<Func<T, bool>> predicate);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> predicate);
        IQueryable<T> Include(Expression<Func<T, object>> include);
    }
}
