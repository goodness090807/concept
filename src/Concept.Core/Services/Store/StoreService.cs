using Concept.Core.Common;
using Concept.Core.Entities.Role.Enums;
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
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            // 檢查區塊
            if ((await storeRepository.GetStoreByUserIdAsync(userId)) != null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserAlreadyHasStore, "使用者已經有商店了");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            // 會用到的資料庫資源寫在這
            storeRepository = _unitOfWork.GetRepository<IStoreRepository>();

            // 資源寫入區塊
            var storeId = await storeRepository.AddStoreAsync(userId, name);
            await userRepository.AddUserStoreRoleAsync(userId, storeId, Roles.Owner);

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
                new(store.Id, store.Name)
            );
        }
    }
}
