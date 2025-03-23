using Concept.Core.Entities;
using Concept.Core.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Concept.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateBaseInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateBaseInfo();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateBaseInfo()
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is ITimestampedEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    }
                }

                var currentUserId = 0;

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
    }
}
