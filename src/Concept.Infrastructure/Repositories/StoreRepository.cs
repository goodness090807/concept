using Concept.Core.Entities.Store;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        public StoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddStoreAsync(int userId, string name)
        {
            var store = new StoreEntity
            {
                UserId = userId,
                Name = name,
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
            return store.Id;
        }
        public async Task<StoreEntity?> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
        }

        public async Task<StoreEntity?> GetStoreByUserIdAsync(int userId)
        {
            return await _context.Stores.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
