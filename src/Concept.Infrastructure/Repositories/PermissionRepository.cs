using Concept.Core.Entities.Permission;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            // 
            bool hasDirectPermission = await _context.UserPermissions
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .AnyAsync(up => up.Permission.Name == permissionName);

            if (hasDirectPermission)
                return true;

            // Check if user has the permission through roles
            bool hasRolePermission = await _context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp)
                .AnyAsync(rp => rp.Permission.Name == permissionName);

            return hasRolePermission;
        }

        public async Task<IEnumerable<PermissionEntity>> GetDirectPermissionsForUserAsync(int userId)
        {
            return await _context.UserPermissions
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<PermissionEntity>> GetRolePermissionsForUserAsync(int userId)
        {
            return await _context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.Permission)
                .Distinct()
                .ToListAsync();
        }
    }
}
