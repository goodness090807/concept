using Concept.API.Attributes;
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
    }
}
