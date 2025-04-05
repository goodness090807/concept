using Concept.API.Controllers.Store.Requests;
using Concept.API.Extensions;
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
    }
}
