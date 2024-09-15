namespace WebAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
