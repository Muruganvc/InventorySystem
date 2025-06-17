using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Domain.Common;
using System.Linq.Expressions;

namespace Stock_Maintenance_System_Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly SmsDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(SmsDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    // Expose IQueryable for advanced queries
    public IQueryable<T> Table => _dbSet.AsQueryable();

    // Basic read operations
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<IReadOnlyList<T>> GetListByAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    // Write operations
    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void Delete(T entity) =>
        _dbSet.Remove(entity);
}
