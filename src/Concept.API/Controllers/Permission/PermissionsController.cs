using Concept.API.Authorizations.PermissionAccess;
using Concept.API.Extensions;
using Concept.Core.Entities.Permission;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.Permission
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(
            IPermissionService permissionService,
            ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all permissions for the current user
        /// </summary>
        /// <returns>List of permissions owned by the user</returns>
        [HttpGet("my-permissions")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PermissionEntity>>> GetMyPermissionsAsync()
        {
            try
            {
                var userId = User.GetUserId();
                var permissions = await _permissionService.GetUserPermissionsAsync(userId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user permissions");
                return StatusCode(500, "An error occurred while retrieving permissions");
            }
        }

        /// <summary>
        /// Tests access to a protected endpoint that requires the Dashboard.View permission
        /// </summary>
        /// <returns>A success message if authorized</returns>
        [HttpGet("test-dashboard-access")]
        [Authorize]
        [RequirePermission("Dashboard.View")]
        public ActionResult<object> TestDashboardAccess()
        {
            return Ok(new { message = "You have access to the Dashboard!" });
        }

        /// <summary>
        /// Tests access to a protected endpoint that requires the Settings.Manage permission
        /// </summary>
        /// <returns>A success message if authorized</returns>
        [HttpGet("test-settings-access")]
        [Authorize]
        [RequirePermission("Settings.Manage")]
        public ActionResult<object> TestSettingsAccess()
        {
            return Ok(new { message = "You have access to manage Settings!" });
        }
    }
}
