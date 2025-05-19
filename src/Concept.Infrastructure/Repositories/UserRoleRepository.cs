using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName)
        {
            return await _context.UserRoles
                .AsNoTracking()
                .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);
        }

        public async Task<bool> UserHasRoleForResourceAsync(int userId, string roleName, string resourceType, int resourceId)
        {
            // 檢查使用者是否擁有指定資源的指定角色
            return await _context.UserRoles
                .AsNoTracking()
                .AnyAsync(ur => 
                    ur.UserId == userId && 
                    ur.Role.Name == roleName); // ResourceId = 0 表示全域角色
        }
    }
}
