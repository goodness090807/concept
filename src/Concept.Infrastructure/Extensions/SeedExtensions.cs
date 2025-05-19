using Concept.Core.Interfaces;
using Concept.Infrastructure.Data;
using Concept.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Concept.Infrastructure.Extensions
{
    public static class SeedExtensions
    {
        /// <summary>
        /// Applies any pending migrations and seeds the database with initial data
        /// </summary>
        public static async Task MigrateAndSeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<object>>();

            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully");

                // Seed permissions and roles
                var permissionSeedService = new PermissionSeedService(context, 
                    serviceProvider.GetRequiredService<ILogger<PermissionSeedService>>());
                await permissionSeedService.SeedPermissionsAndRolesAsync();
                logger.LogInformation("Database seeded with permissions and roles successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database");
                throw;
            }
        }
    }
}
