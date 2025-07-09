using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {


    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin,Mod")]
    public class UserAdminController : ControllerBase {
        private readonly IUserService _userService;



        public UserAdminController(IUserService userService) {
            _userService = userService;
        }



        [HttpGet]
        [Authorize(Policy = "CanViewAllUsers")]
        public async Task<IActionResult> GetAll() {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }




        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewAllUsers")]
        public async Task<IActionResult> GetById(string id) {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }




        [HttpPost("{id}/block")]
        [Authorize(Policy = "CanBanUser")]
        public async Task<IActionResult> BlockUser(string id) {
            await _userService.BlockUserAsync(id);
            return NoContent();
        }




        [HttpPost("{id}/unblock")]
        [Authorize(Policy = "CanBanUser")]
        public async Task<IActionResult> UnblockUser(string id) {
            await _userService.UnblockUserAsync(id);
            return NoContent();
        }









    }

}
