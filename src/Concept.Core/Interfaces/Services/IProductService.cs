using Concept.Core.Common;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Services.Product.ViewModels;

namespace Concept.Core.Interfaces.Services
{
    public interface IProductService : IBaseService
    {
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="name">商品名稱</param>
        /// <param name="description">商品描述</param>
        /// <param name="price">商品價格</param>
        /// <param name="stock">商品庫存</param>
        /// <returns>新增的商品ID</returns>
        Task<Result<int>> AddProductAsync(int storeId, int userId, string name, string description, decimal price, int stock);
        
        /// <summary>
        /// 取得商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品視圖模型</returns>
        Task<Result<ProductViewModel>> GetProductByIdAsync(int productId);

        /// <summary>
        /// 取得商店所有商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <returns>商品列表視圖模型</returns>
        Task<Result<List<ProductViewModel>>> GetProductsByStoreIdAsync(int storeId);

        /// <summary>
        /// 授予使用者商品權限
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="grantedByUserId">授權者ID</param>
        /// <param name="userId">被授權者ID</param>
        /// <param name="permissionLevel">權限等級</param>
        /// <param name="expiresAt">過期時間</param>
        /// <returns>授權ID</returns>
        Task<Result<int>> GrantProductPermissionAsync(int productId, int grantedByUserId, int userId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt);
        
        /// <summary>
        /// 取得商品所屬的商店基本信息（僅包含商品管理者所需信息）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="userId">查詢用戶ID</param>
        /// <returns>商店基本信息</returns>
        Task<Result<ProductManagerStoreViewModel>> GetProductStoreBasicInfoAsync(int productId, int userId);
    }
}
