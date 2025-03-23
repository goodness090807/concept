using Concept.Core.Entities.User;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(string username, string email, string hashedPassword);

        Task<UserEntity?> GetUserByEmailAsync(string email);
    }
}
