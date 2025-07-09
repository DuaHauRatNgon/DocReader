using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {


    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PermissionController : ControllerBase {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService) {
            _permissionService = permissionService;
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetAllPermissions() {
            var result = await _permissionService.GetAllPermissionsAsync();
            return Ok(result);
        }

        [HttpGet("mods")]
        public async Task<IActionResult> GetAllMods() {
            var mods = await _permissionService.GetAllModsLinqAsync();
            return Ok(mods.Select(u => new { u.Id, u.UserName, u.Email }));
        }

        [HttpGet("mods/{id}/permissions")]
        public async Task<IActionResult> GetPermissionsForUser(string id) {
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(id);
            return Ok(permissions);
        }

        [HttpPost("mods/{id}/permissions")]
        public async Task<IActionResult> GrantPermission(string id, [FromBody] PermissionDto dto) {
            await _permissionService.GrantPermissionAsync(id, dto.Value);
            return NoContent();
        }

        [HttpPut("mods/{id}/permissions")]
        public async Task<IActionResult> UpdatePermissions(string id, [FromBody] UpdatePermissionsDto dto) {
            await _permissionService.UpdatePermissionsAsync(id, dto.Permissions);
            return NoContent();
        }

        [HttpDelete("mods/{id}/permissions/{permission}")]
        public async Task<IActionResult> DeletePermission(string id, string permission) {
            await _permissionService.DeletePermissionAsync(id, permission);
            return NoContent();
        }
    }

}
