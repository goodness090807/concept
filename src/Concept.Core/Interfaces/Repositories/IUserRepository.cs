using Concept.Core.Entities.Role.Enums;
using Concept.Core.Entities.User;
using Concept.Core.Entities.User.Enums;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(string username, string email, string hashedPassword, UserStatus userStatus);

        Task<UserEntity?> GetUserByEmailAsync(string email);

        Task<UserEntity?> GetUserByIdAsync(int userId);

        Task AddUserStoreRoleAsync(int userId, int storeId, Roles roleId);

        Task<bool> HasStorePermissionAsync(int userId, int storeId, string permissionName);
    }
}
