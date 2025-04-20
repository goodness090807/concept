using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddResourceAsync(string name, string resourceType, string resourceKey, int ownerId)
        {
            var resource = new ResourceEntity
            {
                Name = name,
                ResourceType = resourceType,
                ResourceKey = resourceKey,
                OwnerId = ownerId
            };

            await _context.Resources.AddAsync(resource);
            await _context.SaveChangesAsync();

            return resource.Id;
        }

        public async Task<int> GrantResourceAccessAsync(int resourceId, int userId, int grantedByUserId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt)
        {
            var resourceAccess = new ResourceAuthorizationEntity
            {
                ResourceId = resourceId,
                AuthorizedUserId = userId,
                GrantedByUserId = grantedByUserId,
                PermissionLevel = permissionLevel,
                // TODO：認為可以再加個開始授權的時間，被授權者可以選擇要從什麼時候開始授權
                ExpiresAt = expiresAt
            };

            await _context.ResourceAuthorizations.AddAsync(resourceAccess);
            await _context.SaveChangesAsync();

            return resourceAccess.Id;
        }

        public async Task<bool> GetPermissionAsync(string resourceType, string resourceKey, int userId, ResourcePermissionLevel permissionLevel, bool checkInactiveResources = false)
        {
            var query = _context.ResourceAuthorizations.AsQueryable();

            if (checkInactiveResources)
            {
                query = query.Where(x => x.IsActive);
            }

            return await query.AnyAsync(x =>
                x.Resource.ResourceType == resourceType &&
                x.Resource.ResourceKey == resourceKey &&
                x.AuthorizedUserId == userId &&
                x.PermissionLevel >= permissionLevel);
        }
    }
}
