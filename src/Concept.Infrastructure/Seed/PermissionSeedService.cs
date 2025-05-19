using Concept.Core.Entities.Permission;
using Concept.Core.Entities.Role;
using Concept.Core.Entities.RolePermission;
using Concept.Core.Extensions;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Concept.Infrastructure.Seed
{
    /// <summary>
    /// Service to seed database with initial permission and role data
    /// </summary>
    public class PermissionSeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionSeedService> _logger;

        public PermissionSeedService(ApplicationDbContext context, ILogger<PermissionSeedService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seeds the database with standard permissions and roles
        /// </summary>
        public async Task SeedPermissionsAndRolesAsync()
        {
            try
            {
                // Create standard permissions
                await SeedStandardPermissionsAsync();
                
                // Create standard roles
                await SeedStandardRolesAsync();
                
                // Assign permissions to roles
                await AssignPermissionsToRolesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding permissions and roles");
                throw;
            }
        }
        
        /// <summary>
        /// Seeds standard permissions to the database
        /// </summary>
        private async Task SeedStandardPermissionsAsync()
        {
            // Get all standard permissions
            var permissions = new List<PermissionEntity>();
            
            // Store management permissions
            permissions.AddRange(GetPermissionEntities(PermissionExtensions.GetCrudPermissions("Stores")));
            
            // User management permissions
            permissions.AddRange(GetPermissionEntities(PermissionExtensions.GetCrudPermissions("Users")));
            
            // Product management permissions
            permissions.AddRange(GetPermissionEntities(PermissionExtensions.GetCrudPermissions("Products")));
            
            // Order management permissions
            permissions.AddRange(GetPermissionEntities(PermissionExtensions.GetCrudPermissions("Orders")));
            
            // Dashboard and report permissions
            permissions.Add(new PermissionEntity { Name = "Dashboard.View", Description = "Access to view the dashboard" });
            permissions.Add(new PermissionEntity { Name = "Reports.View", Description = "Access to view reports" });
            permissions.Add(new PermissionEntity { Name = "Settings.Manage", Description = "Manage application settings" });
            
            foreach (var permission in permissions)
            {
                // Check if permission already exists
                var existingPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == permission.Name);
                
                if (existingPermission == null)
                {
                    _context.Permissions.Add(permission);
                    _logger.LogInformation("Added permission: {PermissionName}", permission.Name);
                }
            }
            
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Seeds standard roles to the database
        /// </summary>
        private async Task SeedStandardRolesAsync()
        {
            var roles = new List<RoleEntity>
            {
                new RoleEntity { Name = "Administrator", Description = "Full system access" },
                new RoleEntity { Name = "StoreManager", Description = "Can manage store operations" },
                new RoleEntity { Name = "StoreViewer", Description = "Read-only access to store data" },
                new RoleEntity { Name = "SalesAgent", Description = "Can manage orders and products" }
            };
            
            foreach (var role in roles)
            {
                // Check if role already exists
                var existingRole = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name == role.Name);
                
                if (existingRole == null)
                {
                    _context.Roles.Add(role);
                    _logger.LogInformation("Added role: {RoleName}", role.Name);
                }
            }
            
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Assigns permissions to roles
        /// </summary>
        private async Task AssignPermissionsToRolesAsync()
        {
            // Administrator - all permissions
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Administrator");
            if (adminRole != null)
            {
                await AssignAllPermissionsToRoleAsync(adminRole.Id);
            }
            
            // StoreManager - store, product, order management
            var storeManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "StoreManager");
            if (storeManagerRole != null)
            {
                await AssignPermissionsToRoleAsync(storeManagerRole.Id, new[] {
                    "Stores.View", "Stores.Update",
                    "Products.View", "Products.Create", "Products.Update", "Products.Delete",
                    "Orders.View", "Orders.Create", "Orders.Update", "Orders.Delete",
                    "Dashboard.View", "Reports.View"
                });
            }
            
            // StoreViewer - view-only permissions
            var storeViewerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "StoreViewer");
            if (storeViewerRole != null)
            {
                await AssignPermissionsToRoleAsync(storeViewerRole.Id, new[] {
                    "Stores.View",
                    "Products.View",
                    "Orders.View"
                });
            }
            
            // SalesAgent - product and order management
            var salesAgentRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "SalesAgent");
            if (salesAgentRole != null)
            {
                await AssignPermissionsToRoleAsync(salesAgentRole.Id, new[] {
                    "Products.View",
                    "Orders.View", "Orders.Create", "Orders.Update"
                });
            }
            
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Assigns all permissions to a role
        /// </summary>
        private async Task AssignAllPermissionsToRoleAsync(int roleId)
        {
            var permissions = await _context.Permissions.ToListAsync();
            foreach (var permission in permissions)
            {
                // Check if permission is already assigned to role
                var exists = await _context.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == roleId && rp.PermissionId == permission.Id);
                
                if (!exists)
                {
                    _context.RolePermissions.Add(new RolePermissionEntity
                    {
                        RoleId = roleId,
                        PermissionId = permission.Id
                    });
                    
                    _logger.LogInformation("Assigned permission {PermissionName} to role ID {RoleId}", 
                        permission.Name, roleId);
                }
            }
        }
        
        /// <summary>
        /// Assigns specific permissions to a role
        /// </summary>
        private async Task AssignPermissionsToRoleAsync(int roleId, string[] permissionNames)
        {
            foreach (var permissionName in permissionNames)
            {
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == permissionName);
                
                if (permission != null)
                {
                    // Check if permission is already assigned to role
                    var exists = await _context.RolePermissions.AnyAsync(rp =>
                        rp.RoleId == roleId && rp.PermissionId == permission.Id);
                    
                    if (!exists)
                    {
                        _context.RolePermissions.Add(new RolePermissionEntity
                        {
                            RoleId = roleId,
                            PermissionId = permission.Id
                        });
                        
                        _logger.LogInformation("Assigned permission {PermissionName} to role ID {RoleId}", 
                            permission.Name, roleId);
                    }
                }
            }
        }
        
        /// <summary>
        /// Helper method to convert permission name strings to PermissionEntity objects
        /// </summary>
        private IEnumerable<PermissionEntity> GetPermissionEntities(string[] permissionNames)
        {
            return permissionNames.Select(name => new PermissionEntity
            {
                Name = name,
                Description = $"Permission to {name.Split('.')[1].ToLower()} {name.Split('.')[0].ToLower()}"
            });
        }
    }
}
