using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entity;

namespace WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            return await _dbSet
                .Where(p => p.ProductName.Contains(productName))
                .ToListAsync();
        }
    }
}
