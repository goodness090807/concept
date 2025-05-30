using Concept.Core.Entities.Role.Enums;
using Concept.Core.Entities.User;
using Concept.Core.Entities.User.Enums;
using Concept.Core.Entities.UserStoreRole;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUserAsync(string username, string email, string hashedPassword, UserStatus userStatus)
        {
            var user = new UserEntity
            {
                Email = email,
                UserName = username,
                HashedPassword = hashedPassword,
                UserStatus = userStatus,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task AddUserStoreRoleAsync(int userId, int storeId, Roles roleId)
        {
            var userStoreRole = new UserStoreRoleEntity
            {
                UserId = userId,
                StoreId = storeId,
                RoleId = (int)roleId
            };

            await _context.UserStoreRoles.AddAsync(userStoreRole);

            await _context.SaveChangesAsync();
        }

        public async Task UpsertUserStoreRolesAsync(int userId, int storeId, IEnumerable<Roles> roleIds)
        {
            // 先刪除舊的角色
            var existingRoles = await _context.UserStoreRoles
                .Where(r => r.UserId == userId && r.StoreId == storeId)
                .ToListAsync();
            _context.UserStoreRoles.RemoveRange(existingRoles);

            // 新增新的角色
            foreach (var roleId in roleIds)
            {
                var userStoreRole = new UserStoreRoleEntity
                {
                    UserId = userId,
                    StoreId = storeId,
                    RoleId = (int)roleId
                };
                await _context.UserStoreRoles.AddAsync(userStoreRole);
            }
        }

        public async Task<bool> HasStorePermissionAsync(int userId, int storeId, string permissionName)
        {
            // 檢查商店角色權限
            var hasPermission = await _context.UserStoreRoles
                .AnyAsync(r => r.UserId == userId && r.StoreId == storeId && r.IsActive
                    && r.Role.RolePermissions.Any(rp => rp.Permission.Name == permissionName));

            if (hasPermission)
            {
                return true;
            }

            // 檢查使用者是否有特定商店的權限
            hasPermission = await _context.UserStorePermissions
                .AnyAsync(p => p.UserId == userId && p.StoreId == storeId && p.Permission.Name == permissionName);

            return hasPermission;
        }
    }
}
