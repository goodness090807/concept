using Concept.Core.Common;
using Concept.Core.Entities.Resource;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;

namespace Concept.Core.Services.Store
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> AddStoreAsync(int userId, string name)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            // 會用到的資料庫資源寫在這
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();

            // 檢查區塊
            if ((await storeRepository.GetStoreByUserIdAsync(userId)) != null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserAlreadyHasStore, "使用者已經有商店了");
            }

            // 資源寫入區塊
            var storeId = await storeRepository.AddStoreAsync(userId, name);
            await resourceRepository.AddResourceAsync(name, ResourceTypes.Store, storeId.ToString(), userId);

            // 提交並回傳
            await transaction.CommitAsync();
            return Result<int>.Success(storeId);
        }
    }
}
