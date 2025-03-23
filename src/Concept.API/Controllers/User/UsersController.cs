using Concept.API.Controllers.User.Requests;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.User
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 新增使用者(有權限者才可以使用)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] RegisterUserRequest req)
        {
            var result = await _userService.AddUserAsync(
                req.Username,
                req.Email,
                req.Password,
                req.ConfirmPassword);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
    }
}
