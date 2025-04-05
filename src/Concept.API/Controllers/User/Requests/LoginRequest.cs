namespace Concept.API.Controllers.User.Requests
{
    public class LoginRequest
    {
        /// <summary>
        /// 使用者Email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
