using Concept.Core.Entities;
using Concept.Core.Entities.Permission;
using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization;
using Concept.Core.Entities.Role;
using Concept.Core.Entities.RolePermission;
using Concept.Core.Entities.Store;
using Concept.Core.Entities.User;
using Concept.Core.Entities.UserPermission;
using Concept.Core.Entities.UserRoleEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace Concept.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<ResourceAuthorizationEntity> ResourceAuthorizations { get; set; }        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<UserPermissionEntity> UserPermissions { get; set; }

        public DbSet<StoreEntity> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateEntitiesInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateEntitiesInfo();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateEntitiesInfo()
        {
            var entries = ChangeTracker.Entries();
            var currentUserId = GetCurrentUserId();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // 更新時間戳
                if (entry.Entity is ITimestampedEntity timestampedEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        timestampedEntity.CreatedAt = currentTime;
                        timestampedEntity.UpdatedAt = currentTime;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        timestampedEntity.UpdatedAt = currentTime;
                    }
                }
                
                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedById = currentUserId;
                        auditableEntity.UpdatedById = currentUserId;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditableEntity.UpdatedById = currentUserId;
                    }
                }
            }
        }

        private int? GetCurrentUserId()
        {
            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated is true)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
            }
            return null;
        }
    }
}
