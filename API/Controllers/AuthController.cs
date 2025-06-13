using Application.DTOs.Auth;
using Application.Services;
using Azure.Core;
using Core.Interfaces;
using Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers {


    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request) {
            var origin = Request.Headers["Origin"];
            var result = await _authService.RegisterAsync(request, origin);
            return Ok(result);
        }





        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            var result = await _authService.ConfirmEmailAsync(userId, token);
            return result ? Ok("Email confirmed") : BadRequest("Error invalid comfirm");
        }





        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }







        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request) {
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }








        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromServices] EmailSender sender) {
            await sender.SendEmailAsync("nmhuongbn@gmail.com", "Test", "Email ogee !");
            return Ok("Sent");
        }







        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request) {
            await _authService.LogoutAsync(request.RefreshToken);
            return Ok(new { message = "Logged out oke" });
        }







        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request) {
            var origin = Request.Headers["Origin"];
            await _authService.ForgotPasswordAsync(request.Email, origin);
            return Ok(new { message = "Link reset passwd da duoc gui" });
        }






        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request) {
            await _authService.ResetPassedAsync(request.Email, request.Token, request.NewPassword);
            return Ok(new { message = "Change password success" });
        }

    }

}
