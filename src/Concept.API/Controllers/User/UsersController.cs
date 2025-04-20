using Concept.API.Controllers.User.Requests;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.User
{
    /// <summary>
    /// 會員相關
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest req)
        {
            var result = await _userService.RegisterAsync(req.Email, req.Username, req.Password, req.ConfirmPassword);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    UserErrorCodes.EmailAlreadyExists => Conflict("Email已存在"),
                    UserErrorCodes.PasswordAndConfirmPasswordNotMatch => BadRequest("密碼與確認密碼不相同"),
                    _ => BadRequest("註冊失敗")
                };
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest req)
        {
            var result = await _userService.LoginAsync(req.Email, req.Password);
            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    UserErrorCodes.UserNotFound => NotFound("使用者不存在"),
                    UserErrorCodes.InvalidPassword => BadRequest("密碼錯誤"),
                    _ => BadRequest("登入失敗")
                };
            }
            return Ok(result.Data);
        }
    }
}
