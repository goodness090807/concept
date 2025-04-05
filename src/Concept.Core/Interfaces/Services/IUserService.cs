using Concept.Core.Common;

namespace Concept.Core.Interfaces.Services
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 使用者註冊
        /// </summary>
        Task<Result<int>> RegisterAsync(string email, string username, string password, string confirmPassword);

        /// <summary>
        /// 使用者登入
        /// </summary>
        Task<Result<string>> LoginAsync(string email, string password);
    }
}
