using Concept.Core.Entities.User;
using Concept.Core.Entities.User.Enums;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(string username, string email, string hashedPassword, UserStatus userStatus);

        Task<UserEntity?> GetUserByEmailAsync(string email);
    }
}
