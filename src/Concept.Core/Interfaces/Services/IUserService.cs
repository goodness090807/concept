using Concept.Core.Common;
using Concept.Core.Services.User.ViewModels;

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

        /// <summary>
        /// 取得使用者可以存取的商店列表
        /// </summary>
        /// <returns>使用者的商店列表，包括自己建立的和被分享的</returns>
        Task<Result<UserStoresViewModel>> GetStoresAsync(int userId);
    }
}
