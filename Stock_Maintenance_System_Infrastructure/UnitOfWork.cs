﻿using InventorySystem_Domain.Common;

namespace InventorySystem_Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly SmsDbContext _context;
    private readonly IUserInfo _userInfo;
    public UnitOfWork(SmsDbContext context, IUserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }
    public IRepository<T> Repository<T>() where T : class
      => new Repository<T>(_context);
    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        await action();
        await transaction.CommitAsync();
    }
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
