using Concept.Core.Entities.Permission;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;

namespace Concept.Core.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            var permissionRepository = _unitOfWork.GetRepository<IPermissionRepository>();
            return await permissionRepository.UserHasPermissionAsync(userId, permissionName);
        }

        public async Task<IEnumerable<PermissionEntity>> GetUserPermissionsAsync(int userId)
        {
            var permissionRepository = _unitOfWork.GetRepository<IPermissionRepository>();
            
            // Get direct permissions
            var directPermissions = await permissionRepository.GetDirectPermissionsForUserAsync(userId);
            
            // Get permissions from roles
            var rolePermissions = await permissionRepository.GetRolePermissionsForUserAsync(userId);
            
            // Combine and remove duplicates
            return directPermissions.Union(rolePermissions);
        }
    }
}
