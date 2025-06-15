using System.Linq.Expressions;

namespace Stock_Maintenance_System_Domain.Common
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
