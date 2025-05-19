using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;

namespace Concept.Core.Services.Role
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            // TODO: 從資料庫檢查使用者是否具有指定角色
            var userRoleRepository = _unitOfWork.GetRepository<IUserRoleRepository>();
            return await userRoleRepository.UserHasRoleAsync(userId, roleName);
        }

        public async Task<bool> HasRoleForResourceAsync(int userId, string roleName, string resourceType, int resourceId)
        {
            // TODO: 從資料庫檢查使用者是否具有指定資源的指定角色
            var userRoleRepository = _unitOfWork.GetRepository<IUserRoleRepository>();
            return await userRoleRepository.UserHasRoleForResourceAsync(userId, roleName, resourceType, resourceId);
        }
    }
}
