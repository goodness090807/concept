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
        
        public async Task<Result<int>> GrantStorePermissionAsync(int storeId, int grantedByUserId, int userId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt)
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

            if (permissionLevel == ResourcePermissionLevel.OWNER)
            {
                return Result<int>.Failure(StoreErrorCodes.CannotGrantOwnerPermission, "不能授予所有者權限");
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

            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();
            // 2. 取得資源ID
            var resourcePermissions = await resourceRepository.GetUserResourcePermissionsAsync(ResourceTypes.Store, grantedByUserId);
            var storeResource = resourcePermissions.FirstOrDefault(r => r.ResourceKey == storeId.ToString());

            if (storeResource.ResourceId <= 0)
            {
                return Result<int>.Failure(StoreErrorCodes.ResourceNotFound, "找不到商店對應的資源");
            }

            var userPermissions = await resourceRepository.GetUserResourcePermissionsAsync(ResourceTypes.Store, userId, includeExpired: true);

            // 3. 授予權限
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                int authorizationId;

                // 檢查用戶是否已經有權限（避免重複授權）
                var existingPermission = userPermissions.FirstOrDefault(r => r.ResourceKey == storeId.ToString());
                if (existingPermission.ResourceId > 0)
                {
                    if (existingPermission.ExpiresAt.HasValue && existingPermission.ExpiresAt < DateTime.UtcNow)
                    {
                        // 權限已過期，重新授權
                        authorizationId = await resourceRepository.GrantResourceAccessAsync(
                            storeResource.ResourceId,
                            userId,
                            grantedByUserId,
                            permissionLevel,
                            expiresAt);
                    }
                    else
                    {
                        return Result<int>.Failure(StoreErrorCodes.PermissionAlreadyExists, "該用戶已經擁有此商店的權限");
                    }
                }
                else
                {
                    // 用戶沒有此商店的權限，新增授權
                    authorizationId = await resourceRepository.GrantResourceAccessAsync(
                        storeResource.ResourceId,
                        userId,
                        grantedByUserId,
                        permissionLevel,
                        expiresAt);
                }

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
