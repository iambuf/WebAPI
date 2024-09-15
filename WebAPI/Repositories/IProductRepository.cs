using WebAPI.Entity;

namespace WebAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductByName(string productName);
    }
}
