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
            await userRepository.AddUserStoreRoleAsync(userId, storeId, Roles.StoreOwner);

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

        public async Task<Result> GrantStoreRoleAsync(int storeId, int userId, IEnumerable<Roles> roles)
        {
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            // 檢查區塊
            if ((await storeRepository.GetStoreByIdAsync(storeId)) == null)
            {
                return Result<int>.Failure(StoreErrorCodes.StoreNotFound, "商店不存在");
            }

            if ((await userRepository.GetUserByIdAsync(userId)) == null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserNotFound, "使用者不存在");
            }

            if (roles.Any(x => x == Roles.StoreOwner))
            {
                return Result.Failure(StoreErrorCodes.CannotGrantOwnerPermission, "無法授予商店擁有者權限");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            // 資源寫入區塊
            await userRepository.UpsertUserStoreRolesAsync(userId, storeId, roles);

            // 提交並回傳
            await transaction.CommitAsync();
            return Result.Success();
        }
    }
}
