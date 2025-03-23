using Concept.Core.Common;

namespace Concept.Core.Interfaces.Services
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 註冊使用者
        /// </summary>
        Task<Result<int>> RegisterUserAsync(string email, string password, string confirmPassword);

        /// <summary>
        /// 新增使用者
        /// </summary>
        Task<Result<int>> AddUserAsync(string username, string email, string password, string confirmPassword);
    }
}
