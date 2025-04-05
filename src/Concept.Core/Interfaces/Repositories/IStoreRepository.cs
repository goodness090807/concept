using Concept.Core.Entities.Store;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IStoreRepository
    {
        Task<int> AddStoreAsync(int userId, string name);
        Task<StoreEntity?> GetStoreByIdAsync(int storeId);
        Task<StoreEntity?> GetStoreByUserIdAsync(int userId);
    }
}
