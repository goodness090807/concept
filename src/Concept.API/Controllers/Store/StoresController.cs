using Concept.API.Authorizations.ResourceAccess;
using Concept.API.Controllers.Store.Requests;
using Concept.API.Extensions;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.Store
{
    public class StoresController : BaseController
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService) 
        {
            _storeService = storeService;
        }

        /// <summary>
        /// 新增商店
        /// </summary>
        [HttpPost, Authorize]
        public async Task<IActionResult> AddStoreAsync([FromBody] AddStoreRequest req)
        {
            var result = await _storeService.AddStoreAsync(User.GetUserId(), req.Name);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    StoreErrorCodes.UserAlreadyHasStore => Conflict("使用者已經有商店了"),
                    _ => BadRequest("新增商店失敗")
                };
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// 取得商店資訊
        /// </summary>
        [HttpGet("{storeId}"), Authorize]
        [ResourceAccess("Store", "StoreId", ResourcePermissionLevel.VIEWER)]
        public async Task<IActionResult> GetStoreByIdAsync(int storeId)
        {
            var result = await _storeService.GetStoreByIdAsync(storeId);
            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    StoreErrorCodes.StoreNotFound => NotFound("商店不存在"),
                    _ => BadRequest("取得商店失敗")
                };
            }
            return Ok(result.Data);
        }
        
        /// <summary>
        /// 更新商店資訊
        /// </summary>
        [HttpPut("{storeId}"), Authorize]
        [ResourceAccess("Store", "StoreId", ResourcePermissionLevel.ADMIN)]
        public async Task<IActionResult> UpdateStoreAsync([FromRoute]int storeId, [FromBody] UpdateStoreRequest req)
        {
            var result = await _storeService.UpdateStoreAsync(storeId, req.Name, req.Description);
            
            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    StoreErrorCodes.StoreNotFound => NotFound("商店不存在"),
                    StoreErrorCodes.InvalidData => BadRequest("商店名稱不能為空"),
                    StoreErrorCodes.UpdateFailed => BadRequest("更新商店資料失敗"),
                    _ => BadRequest("更新商店失敗")
                };
            }
            
            return Ok(result.Data);
        }
        
        /// <summary>
        /// 授予使用者商店權限
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="req">授權請求詳情</param>
        /// <returns>授權ID</returns>
        [HttpPost("{storeId}/permissions"), Authorize]
        [ResourceAccess("Store", "StoreId", ResourcePermissionLevel.ADMIN)]
        public async Task<IActionResult> GrantStorePermissionAsync(int storeId, [FromBody] GrantStorePermissionRequest req)
        {
            var result = await _storeService.GrantStorePermissionAsync(
                storeId,
                User.GetUserId(),
                req.UserId,
                req.PermissionLevel,
                req.ExpiresAt);

            if (result.IsFailure)
            {                return result.ErrorCode switch
                {
                    StoreErrorCodes.StoreNotFound => NotFound("商店不存在"),
                    StoreErrorCodes.InvalidUserId => BadRequest("被授權使用者ID無效"),
                    StoreErrorCodes.UserNotFound => NotFound("被授權的使用者不存在"),
                    StoreErrorCodes.CannotGrantPermissionToSelf => BadRequest("不能授予自己權限"),
                    StoreErrorCodes.CannotGrantOwnerPermission => BadRequest("不能授予所有者權限"),
                    StoreErrorCodes.InsufficientPermission => Forbid("您沒有足夠的權限進行授權"),
                    StoreErrorCodes.ResourceNotFound => NotFound("找不到商店對應的資源"),
                    StoreErrorCodes.PermissionAlreadyExists => Conflict("該用戶已經擁有此商店的權限"),
                    _ => BadRequest("授予權限失敗")
                };
            }
            
            return Ok(result.Data);
        }
    }
}
