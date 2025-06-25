namespace Stock_Maintenance_System_Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenericRepository<Product> Products { get; }
        //IGenericRepository<Category> Categories { get; }

        Task<int> SaveAsync();
        IRepository<T> Repository<T>() where T : class;

        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken);
    }
}
