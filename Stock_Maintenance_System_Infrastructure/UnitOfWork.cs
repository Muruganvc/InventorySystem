using Stock_Maintenance_System_Domain.Common;
using Stock_Maintenance_System_Domain.User;

namespace Stock_Maintenance_System_Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly SmsDbContext _context;
    public UnitOfWork(SmsDbContext context)
    {
        _context = context;
    }
    public IRepository<T> Repository<T>() where T : class
      => new Repository<T>(_context);
    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        await action();
        await transaction.CommitAsync();
    }
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
