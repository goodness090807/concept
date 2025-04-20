using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Core.Interfaces.Services;

namespace Concept.Core.Services.Resource
{
    public class ResourcePermissionService : IResourcePermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResourcePermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> HasPermissionAsync(string resourceType, string resourceKey, int userId, ResourcePermissionLevel resourcePermissionLevel, bool checkInactiveResources = false)
        {
            // Optimization: 確認是否需要使用Result Pattern
            var resourcePermissionRepository = _unitOfWork.GetRepository<IResourceRepository>();
            return resourcePermissionRepository.GetPermissionAsync(resourceType, resourceKey, userId, resourcePermissionLevel, checkInactiveResources);
        }
    }
}