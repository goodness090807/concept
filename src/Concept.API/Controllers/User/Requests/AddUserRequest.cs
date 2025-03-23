using System.ComponentModel.DataAnnotations;

namespace Concept.API.Controllers.User.Requests
{
    public class RegisterUserRequest
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
