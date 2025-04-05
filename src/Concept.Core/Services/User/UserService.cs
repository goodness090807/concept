using Concept.Core.Common;
using Concept.Core.Entities.User.Enums;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;
using Shared.Interfaces;

namespace Concept.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<int>> RegisterAsync(string email, string username, string password, string confirmPassword)
        {
            // 會用到的資料庫資源寫在這
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            // 檢查區塊
            if (password != confirmPassword)
            {
                return Result<int>.Failure(UserErrorCodes.PasswordAndConfirmPasswordNotMatch, "密碼不一致");
            }

            if (await userRepository.GetUserByEmailAsync(email) != null)
            {
                return Result<int>.Failure(UserErrorCodes.EmailAlreadyExists, "Email已存在");
            }

            // 邏輯處理
            var hashedPassword = _passwordHasher.HashPassword(password);

            // 資源寫入區塊
            var userId = await userRepository.AddUserAsync(username, email, hashedPassword, UserStatus.Inactive);

            // 提交並回傳(只有一個資料庫操作，所以不需要 transaction和提交)
            return Result<int>.Success(userId);
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            // 會用到的資料庫資源寫在這
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            // 檢查區塊
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Result<string>.Failure(UserErrorCodes.UserNotFound, "使用者不存在");
            }
            if (!_passwordHasher.VerifyPassword(user.HashedPassword, password))
            {
                return Result<string>.Failure(UserErrorCodes.InvalidPassword, "密碼錯誤");
            }

            // 取得token
            var token = _tokenService.GenerateJwtToken(user.Id.ToString(), new List<System.Security.Claims.Claim>());

            // 提交並回傳(只有一個資料庫操作，所以不需要 transaction和提交)
            return Result<string>.Success(token);
        }
    }
}
