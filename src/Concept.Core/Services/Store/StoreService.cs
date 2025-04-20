using Concept.Core.Common;
using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.Store.ViewModels;

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
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            // 檢查區塊
            if ((await storeRepository.GetStoreByUserIdAsync(userId)) != null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserAlreadyHasStore, "使用者已經有商店了");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            // 會用到的資料庫資源寫在這
            storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();


            // 資源寫入區塊
            var storeId = await storeRepository.AddStoreAsync(userId, name);
            var resourceId = await resourceRepository.AddResourceAsync(name, ResourceTypes.Store, storeId.ToString(), userId);
            await resourceRepository.GrantResourceAccessAsync(resourceId, userId, userId, ResourcePermissionLevel.OWNER, null);

            // 提交並回傳
            await transaction.CommitAsync();
            return Result<int>.Success(storeId);
        }

        public async Task<Result<StoreViewModel>> GetStoreByIdAsync(int storeId)
        {
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var store = await storeRepository.GetStoreByIdAsync(storeId);
            if (store == null)
            {
                return Result<StoreViewModel>.Failure(StoreErrorCodes.StoreNotFound, "商店不存在");
            }

            return Result<StoreViewModel>.Success(
                new StoreViewModel(
                        store.Id,
                        store.Name
                )
            );
        }
    }
}
