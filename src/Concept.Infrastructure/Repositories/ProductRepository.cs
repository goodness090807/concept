using Concept.Core.Entities.Product;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddProductAsync(int storeId, string name, string description, decimal price, int stock)
        {
            var product = new ProductEntity
            {
                StoreId = storeId,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<ProductEntity>> GetProductsByStoreIdAsync(int storeId)
        {
            return await _context.Products
                .Where(p => p.StoreId == storeId)
                .ToListAsync();
        }
    }
}
