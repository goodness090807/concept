using Concept.Core.Common;
using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.Product.ViewModels;
using Concept.Core.Services.Store;

namespace Concept.Core.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> AddProductAsync(int storeId, int userId, string name, string description, decimal price, int stock)
        {
            // 驗證商店存在
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var store = await storeRepository.GetStoreByIdAsync(storeId);
            if (store == null)
            {
                return Result<int>.Failure(ProductErrorCodes.StoreNotFound, "商店不存在");
            }

            // 驗證價格和庫存
            if (price < 0)
            {
                return Result<int>.Failure(ProductErrorCodes.InvalidPrice, "商品價格不得為負數");
            }

            if (stock < 0)
            {
                return Result<int>.Failure(ProductErrorCodes.InvalidStock, "商品庫存不得為負數");
            }

            // 驗證用戶對商店的權限：商店管理者或商品管理者都可以添加商品
            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();
            
            // 檢查是否有商店管理員權限
            var hasAdminPermission = await resourceRepository.GetPermissionAsync(
                ResourceTypes.Store,
                storeId.ToString(),
                userId,
                ResourcePermissionLevel.ADMIN);

            // 檢查是否有商品管理員權限
            var hasProductManagerPermission = await resourceRepository.GetPermissionAsync(
                ResourceTypes.Store,
                storeId.ToString(),
                userId,
                ResourcePermissionLevel.PRODUCT_MANAGER);

            if (!hasAdminPermission && !hasProductManagerPermission)
            {
                return Result<int>.Failure(ProductErrorCodes.InsufficientPermission, "您沒有權限在此商店添加商品");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 添加商品
                var productRepository = _unitOfWork.GetRepository<IProductRepository>();
                var productId = await productRepository.AddProductAsync(storeId, name, description, price, stock);

                // 創建商品資源並給予權限
                var resourceId = await resourceRepository.AddResourceAsync(name, ResourceTypes.Product, productId.ToString(), userId);
                
                // 給創建者授予商品管理員權限
                await resourceRepository.GrantResourceAccessAsync(resourceId, userId, userId, ResourcePermissionLevel.PRODUCT_MANAGER, null);
                
                // 提交並回傳
                await transaction.CommitAsync();
                return Result<int>.Success(productId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ProductErrorCodes.ProductAddFailed, $"新增商品失敗: {ex.Message}");
            }
        }

        public async Task<Result<ProductViewModel>> GetProductByIdAsync(int productId)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            var product = await productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Result<ProductViewModel>.Failure(ProductErrorCodes.ProductNotFound, "商品不存在");
            }

            return Result<ProductViewModel>.Success(new ProductViewModel(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.StoreId
            ));
        }

        public async Task<Result<List<ProductViewModel>>> GetProductsByStoreIdAsync(int storeId)
        {
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var store = await storeRepository.GetStoreByIdAsync(storeId);
            
            if (store == null)
            {
                return Result<List<ProductViewModel>>.Failure(ProductErrorCodes.StoreNotFound, "商店不存在");
            }

            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            var products = await productRepository.GetProductsByStoreIdAsync(storeId);

            var productViewModels = products.Select(p => new ProductViewModel(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.StoreId
            )).ToList();

            return Result<List<ProductViewModel>>.Success(productViewModels);
        }

        public async Task<Result<int>> GrantProductPermissionAsync(int productId, int grantedByUserId, int userId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt)
        {
            // 驗證參數
            if (grantedByUserId <= 0)
            {
                return Result<int>.Failure(ProductErrorCodes.InvalidGrantedByUserId, "授權者ID無效");
            }

            if (userId <= 0)
            {
                return Result<int>.Failure(ProductErrorCodes.InvalidUserId, "被授權使用者ID無效");
            }

            if (userId == grantedByUserId)
            {
                return Result<int>.Failure(ProductErrorCodes.CannotGrantPermissionToSelf, "不能授予自己權限");
            }

            if (permissionLevel == ResourcePermissionLevel.OWNER)
            {
                return Result<int>.Failure(ProductErrorCodes.CannotGrantOwnerPermission, "不能授予所有者權限");
            }

            // 驗證使用者是否存在
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Result<int>.Failure(ProductErrorCodes.UserNotFound, "被授權的使用者不存在");
            }

            // 1. 檢查商品是否存在
            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return Result<int>.Failure(ProductErrorCodes.ProductNotFound, "商品不存在");
            }

            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();
            
            // 2. 取得資源ID
            var resourcePermissions = await resourceRepository.GetUserResourcePermissionsAsync(ResourceTypes.Product, grantedByUserId);
            var productResource = resourcePermissions.FirstOrDefault(r => r.ResourceKey == productId.ToString());
            
            if (productResource.ResourceId <= 0)
            {
                return Result<int>.Failure(ProductErrorCodes.ResourceNotFound, "找不到商品對應的資源");
            }
              // 3. 檢查授權者的權限: 需要ADMIN或PRODUCT_MANAGER權限
            var hasAdminPermission = await resourceRepository.GetPermissionAsync(
                ResourceTypes.Product,
                productId.ToString(),
                grantedByUserId,
                ResourcePermissionLevel.ADMIN);
                
            var hasProductManagerPermission = await resourceRepository.GetPermissionAsync(
                ResourceTypes.Product,
                productId.ToString(),
                grantedByUserId,
                ResourcePermissionLevel.PRODUCT_MANAGER);

            if (!hasAdminPermission && !hasProductManagerPermission)
            {
                return Result<int>.Failure(ProductErrorCodes.InsufficientPermission, "您沒有足夠的權限進行授權");
            }
            
            // 4. 授予權限
            var userPermissions = await resourceRepository.GetUserResourcePermissionsAsync(ResourceTypes.Product, userId, includeExpired: true);
            
            int authorizationId;

            try
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync();
                  // 檢查用戶是否已經有此商品的權限
                var userProductPermission = userPermissions.FirstOrDefault(r => r.ResourceKey == productId.ToString());
                
                if (userProductPermission.ResourceId > 0)
                {
                    // 用戶已有此商品的權限，檢查是否過期或需要更新
                    if (userProductPermission.ExpiresAt.HasValue && userProductPermission.ExpiresAt.Value < DateTime.UtcNow || 
                        userProductPermission.PermissionLevel != permissionLevel)
                    {
                        // 權限已過期或權限等級不同，更新權限
                        authorizationId = await resourceRepository.GrantResourceAccessAsync(
                            productResource.ResourceId,
                            userId,
                            grantedByUserId,
                            permissionLevel,
                            expiresAt);
                    }
                    else
                    {
                        return Result<int>.Failure(ProductErrorCodes.PermissionAlreadyExists, "該用戶已經擁有此商品的權限");
                    }
                }
                else
                {
                    // 用戶沒有此商品的權限，新增授權
                    authorizationId = await resourceRepository.GrantResourceAccessAsync(
                        productResource.ResourceId,
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
                return Result<int>.Failure(ProductErrorCodes.PermissionGrantFailed, $"授予權限失敗: {ex.Message}");
            }
        }

        public async Task<Result<ProductManagerStoreViewModel>> GetProductStoreBasicInfoAsync(int productId, int userId)
        {
            // 1. 檢查商品是否存在
            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return Result<ProductManagerStoreViewModel>.Failure(ProductErrorCodes.ProductNotFound, "商品不存在");
            }

            // 2. 檢查用戶是否有權限訪問該商品
            var resourceRepository = _unitOfWork.GetRepository<IResourceRepository>();
            var hasProductViewPermission = await resourceRepository.GetPermissionAsync(
                ResourceTypes.Product,
                productId.ToString(),
                userId,
                ResourcePermissionLevel.VIEWER);

            if (!hasProductViewPermission)
            {
                return Result<ProductManagerStoreViewModel>.Failure(ProductErrorCodes.InsufficientPermission, "您沒有權限查看此商品信息");
            }

            // 3. 獲取商店的基本信息
            var storeRepository = _unitOfWork.GetRepository<IStoreRepository>();
            var store = await storeRepository.GetStoreByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ProductManagerStoreViewModel>.Failure(ProductErrorCodes.StoreNotFound, "找不到商品所屬的商店");
            }

            // 4. 返回商店的基本信息（僅ID和名稱）
            var storeBasicInfo = new ProductManagerStoreViewModel(store.Id, store.Name);
            return Result<ProductManagerStoreViewModel>.Success(storeBasicInfo);
        }
    }
}
