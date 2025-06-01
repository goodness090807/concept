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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;

        public StoreService(IUnitOfWork unitOfWork, IStoreRepository storeRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<int>> AddStoreAsync(int userId, string name)
        {
            // 檢查區塊
            if ((await _storeRepository.GetStoreByUserIdAsync(userId)) != null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserAlreadyHasStore, "使用者已經有商店了");
            }

            using var transaction = _unitOfWork.BeginTransaction();

            // 資源寫入區塊
            var storeId = await _storeRepository.AddStoreAsync(userId, name);
            await _userRepository.AddUserStoreRoleAsync(userId, storeId, Roles.StoreOwner);

            // 提交並回傳
            transaction.Commit();

            return Result<int>.Success(storeId);
        }

        public async Task<Result<StoreViewModel>> GetStoreByIdAsync(int storeId)
        {
            var store = await _storeRepository.GetStoreByIdAsync(storeId);
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
            // 檢查區塊
            if ((await _storeRepository.GetStoreByIdAsync(storeId)) == null)
            {
                return Result<int>.Failure(StoreErrorCodes.StoreNotFound, "商店不存在");
            }

            if ((await _userRepository.GetUserByIdAsync(userId)) == null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserNotFound, "使用者不存在");
            }

            if (roles.Any(x => x == Roles.StoreOwner))
            {
                return Result.Failure(StoreErrorCodes.CannotGrantOwnerPermission, "無法授予商店擁有者權限");
            }

            using var transaction = _unitOfWork.BeginTransaction();

            // 資源寫入區塊
            await _userRepository.UpsertUserStoreRolesAsync(userId, storeId, roles);

            // 提交並回傳
            transaction.Commit();
            return Result.Success();
        }
    }
}
