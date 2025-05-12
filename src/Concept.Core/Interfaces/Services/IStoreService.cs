using Concept.Core.Common;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Services.Store.ViewModels;

namespace Concept.Core.Interfaces.Services
{
    public interface IStoreService : IBaseService
    {
        /// <summary>
        /// 新增商店
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Result<int>> AddStoreAsync(int userId, string name);

        /// <summary>
        /// 取得商店資訊
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        Task<Result<StoreViewModel>> GetStoreByIdAsync(int storeId);
        
        /// <summary>
        /// 授予使用者商店權限
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="grantedByUserId">授權者ID</param>
        /// <param name="userId">被授權者ID</param>
        /// <param name="permissionLevel">權限等級</param>
        /// <param name="expiresAt">過期時間 (可選)</param>
        /// <returns>授權ID</returns>
        Task<Result<int>> GrantStorePermissionAsync(int storeId, int grantedByUserId, int userId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt);
    }
}
