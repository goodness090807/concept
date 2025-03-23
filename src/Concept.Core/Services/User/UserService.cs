using Concept.Core.Common;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;
using Shared.Interfaces;

namespace Concept.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public Task<Result<int>> RegisterUserAsync(string email, string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<int>> AddUserAsync(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return Result<int>.Failure("密碼與確認密碼不一致");
            }

            if (await _userRepository.GetUserByEmailAsync(email) != null)
            {
                return Result<int>.Failure("Email已存在");
            }

            var hashedPassword = _passwordHasher.HashPassword(password);

            var id = await _userRepository.AddUserAsync(username, email, hashedPassword);

            return Result<int>.Success(id);
        }
    }
}
