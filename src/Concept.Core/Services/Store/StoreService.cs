using Concept.Core.Common;
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


            // 資源寫入區塊
            var storeId = await storeRepository.AddStoreAsync(userId, name);

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
        
        public async Task<Result<int>> GrantStorePermissionAsync(int storeId, int grantedByUserId, int userId, DateTime? expiresAt)
        {
            // 驗證參數
            if (storeId <= 0)
            {
                return Result<int>.Failure(StoreErrorCodes.StoreNotFound, "商店ID無效");
            }

            if (userId <= 0)
            {
                return Result<int>.Failure(StoreErrorCodes.InvalidUserId, "被授權使用者ID無效");
            }

            if (grantedByUserId <= 0)
            {
                return Result<int>.Failure(StoreErrorCodes.InvalidGrantedByUserId, "授權者ID無效");
            }

            if (userId == grantedByUserId)
            {
                return Result<int>.Failure(StoreErrorCodes.CannotGrantPermissionToSelf, "不能授予自己權限");
            }

            // 檢查被授權的使用者是否存在
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var authorizedUser = await userRepository.GetUserByIdAsync(userId);
            if (authorizedUser == null)
            {
                return Result<int>.Failure(StoreErrorCodes.UserNotFound, "被授權的使用者不存在");
            }

            // 1. 檢查商店是否存在
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var store = await storeRepository.GetStoreByIdAsync(storeId);
            if (store == null)
            {
                return Result<int>.Failure(StoreErrorCodes.StoreNotFound, "商店不存在");
            }


            // 3. 授予權限
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                int authorizationId = 0;


                await transaction.CommitAsync();
                return Result<int>.Success(authorizationId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(StoreErrorCodes.PermissionGrantFailed, $"授予權限失敗: {ex.Message}");
            }
        }
    }
}
