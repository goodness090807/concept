namespace Concept.Core.Extensions
{
    /// <summary>
    /// Extension methods for generating standard API permissions
    /// </summary>
    public static class PermissionExtensions
    {
        /// <summary>
        /// Generates a standard permission name for a resource action
        /// </summary>
        /// <param name="resourceName">Resource name (e.g., "Stores", "Users", "Products")</param>
        /// <param name="action">Action name (e.g., "View", "Create", "Update", "Delete")</param>
        /// <returns>A standardized permission name</returns>
        public static string FormatPermission(string resourceName, string action)
        {
            return $"{resourceName}.{action}";
        }

        /// <summary>
        /// Generates standard CRUD permissions for a resource
        /// </summary>
        /// <param name="resourceName">Resource name (e.g., "Stores", "Users", "Products")</param>
        /// <returns>An array of standard CRUD permission names</returns>
        public static string[] GetCrudPermissions(string resourceName)
        {
            return new[]
            {
                FormatPermission(resourceName, "View"),
                FormatPermission(resourceName, "Create"),
                FormatPermission(resourceName, "Update"),
                FormatPermission(resourceName, "Delete")
            };
        }
        
        /// <summary>
        /// Generates standard read-only permissions for a resource
        /// </summary>
        /// <param name="resourceName">Resource name (e.g., "Stores", "Users", "Products")</param>
        /// <returns>An array of standard read-only permission names</returns>
        public static string[] GetReadPermissions(string resourceName)
        {
            return new[]
            {
                FormatPermission(resourceName, "View"),
                FormatPermission(resourceName, "List")
            };
        }
    }
}
