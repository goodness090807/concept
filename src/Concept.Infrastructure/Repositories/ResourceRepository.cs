using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure.Data;

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
            var result = await _context.Resources.AddAsync(new ResourceEntity
            {
                Name = name,
                ResourceType = resourceType,
                ResourceKey = resourceKey,
                OwnerId = ownerId
            });

            await _context.SaveChangesAsync();

            return result.Entity.Id;
        }

        public Task<int> GrantResourceAccessAsync(int resourceId, int userId, int grantedByUserId, ResourcePermissionLevel permissionLevel, DateTime grantedAt, DateTime? expiresAt)
        {
            throw new NotImplementedException();
        }
    }
}
