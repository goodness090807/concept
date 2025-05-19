using Concept.Core.Extensions;

namespace Concept.Core.Services.Permission
{
    /// <summary>
    /// Service for seeding standard API permissions
    /// </summary>
    public class PermissionSeedService
    {
        /// <summary>
        /// Get all standard API permissions for the application
        /// </summary>
        /// <returns>A list of permission names</returns>
        public static List<string> GetAllStandardPermissions()
        {
            var permissions = new List<string>();
            
            // Add Store permissions
            permissions.AddRange(PermissionExtensions.GetCrudPermissions("Stores"));
            
            // Add User permissions
            permissions.AddRange(PermissionExtensions.GetCrudPermissions("Users"));
            
            // Add Product permissions
            permissions.AddRange(PermissionExtensions.GetCrudPermissions("Products"));
            
            // Add Order permissions
            permissions.AddRange(PermissionExtensions.GetCrudPermissions("Orders"));
            
            // Add additional specific permissions as needed
            permissions.Add("Dashboard.View");
            permissions.Add("Reports.View");
            permissions.Add("Settings.Manage");
            
            return permissions;
        }
    }
}
