using Concept.Core.Common;
using Concept.Core.Entities.User.Enums;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.Store;
using Concept.Core.Services.User.ViewModels;
using Shared.Interfaces;
using System.Security.Claims;

namespace Concept.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserService(
            IUserRepository userRepository, IStoreRepository storeRepository,
            IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<int>> RegisterAsync(string email, string username, string password, string confirmPassword)
        {
            // 檢查區塊
            if (password != confirmPassword)
            {
                return Result<int>.Failure(UserErrorCodes.PasswordAndConfirmPasswordNotMatch, "密碼不一致");
            }

            if (await _userRepository.GetUserByEmailAsync(email) != null)
            {
                return Result<int>.Failure(UserErrorCodes.EmailAlreadyExists, "Email已存在");
            }

            // 邏輯處理
            var hashedPassword = _passwordHasher.HashPassword(password);

            // 資源寫入區塊
            var userId = await _userRepository.AddUserAsync(username, email, hashedPassword, UserStatus.Inactive);

            // 提交並回傳(只有一個資料庫操作，所以不需要 transaction和提交)
            return Result<int>.Success(userId);
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            // 檢查區塊
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Result<string>.Failure(UserErrorCodes.UserNotFound, "使用者不存在");
            }
            if (!_passwordHasher.VerifyPassword(user.HashedPassword, password))
            {
                return Result<string>.Failure(UserErrorCodes.InvalidPassword, "密碼錯誤");
            }

            // 取得token
            var token = _tokenService.GenerateJwtToken(user.Id.ToString(), new List<Claim>());

            // 提交並回傳(只有一個資料庫操作，所以不需要 transaction和提交)
            return Result<string>.Success(token);
        }

        public async Task<Result<UserStoresViewModel>> GetStoresAsync(int userId)
        {
            // TODO：查詢使用者可看到的商店列表，包括自己建立的和被分享的
            throw new NotImplementedException();
        }
        
        
        public async Task<Result<bool>> HasStorePermissionAsync(int userId, int storeId, string permissionName)
        {
            var store = await _storeRepository.GetStoreByIdAsync(storeId);
            if (store == null)
            {
                return Result<bool>.Failure(StoreErrorCodes.StoreNotFound, "商店不存在");
            }

            // 檢查使用者是否有權限
            var hasPermission = await _userRepository.HasStorePermissionAsync(userId, storeId, permissionName);
            return Result<bool>.Success(hasPermission);
        }
    }
}
