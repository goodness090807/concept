using Concept.API.Controllers.Authorization.Requests;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.Authorization
{
    /// <summary>
    /// 授權相關(通常是與使用者的操作有關，如註冊、變更密碼等)
    /// </summary>
    public class AuthorizationController : BaseController
    {
        private readonly IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest req)
        {
            var result = await _userService.RegisterUserAsync(
                req.Email, req.Password, req.ConfirmPassword
            );

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
    }
}
