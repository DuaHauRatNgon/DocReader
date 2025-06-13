using Application.DTOs.Auth;
using Azure.Core;
using Core.Interfaces;
using Core.Models.Identity;
using Infrastructure.Email;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {



    public class AuthService {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly EmailSender _emailSender;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;




        public AuthService(UserManager<AppUser> userManager,
                            RoleManager<AppRole> roleManager,
                            ITokenGenerator tokenGenerator,
                            EmailSender emailSender,
                            AppDbContext context,
                            IConfiguration config
                            ) {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
            _context = context;
            _config = config;
        }



        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, string origin) {
            var user = new AppUser {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.DisplayName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception("Tạo user thất bại");

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmLink = $"{origin}/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Click here: {confirmLink}");

            return new AuthResponse { AccessToken = "", RefreshToken = "", ExpiresAt = DateTime.UtcNow };
        }










        public async Task<bool> ConfirmEmailAsync(string userId, string token) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }







        public async Task<AuthResponse> LoginAsync(LoginRequest request) {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password)) {
                Console.WriteLine($"{user.EmailConfirmed}");
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            //var accessToken = await _tokenGenerator.GenerateAccessTokenAsync(user);
            //var refreshToken = await _tokenGenerator.GenerateRefreshTokenAsync(user.Id);

            //_context.RefreshTokens.Add(refreshToken);
            //await _context.SaveChangesAsync();

            //return new AuthResponse {
            //    AccessToken = accessToken,
            //    RefreshToken = refreshToken.Token,
            //    ExpiresAt = refreshToken.Expires
            //};

            var accessToken = await _tokenGenerator.GenerateAccessTokenAsync(user);
            var (token, expires) = await _tokenGenerator.GenerateRefreshTokenAsync(user.Id);

            var refreshToken = new RefreshToken {
                Token = token,
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = expires
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponse {
                AccessToken = accessToken,
                RefreshToken = token,
                ExpiresAt = expires
            };
        }








        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request) {
            var refreshToken = await _context.RefreshTokens         // 1 tìm refresh token trong DB
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
            //Console.WriteLine(refreshToken.Token);
            //Console.WriteLine(refreshToken.User);

            if (refreshToken == null || !refreshToken.IsActive)
                throw new SecurityTokenException("Token không hợp lệ hoặc đã hết hạn.");


            refreshToken.Revoked = DateTime.UtcNow;             // 2 danh dau token cũ la revoked
            _context.RefreshTokens.Update(refreshToken);

            var user = refreshToken.User;

            // 3 gen new access token và refresh token
            var accessToken = await _tokenGenerator.GenerateAccessTokenAsync(user);
            var (newRefreshToken, expires) = await _tokenGenerator.GenerateRefreshTokenAsync(user.Id);

            // 4 save refresh token into db
            var newRefreshTokenEntity = new RefreshToken {
                Token = newRefreshToken,
                Expires = expires,
                UserId = user.Id,
                Created = DateTime.UtcNow,
            };
            await _context.RefreshTokens.AddAsync(newRefreshTokenEntity);

            await _context.SaveChangesAsync();

            // 5 AuthResponse
            return new AuthResponse {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = expires
            };
        }








        public async Task LogoutAsync(string refreshTokenValue) {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshTokenValue);

            if (refreshToken == null) throw new KeyNotFoundException("Sai refresh token roii !!!");

            //refreshToken.IsRevoked = true;
            //refreshToken.RevokedAt = DateTime.UtcNow;

            refreshToken.Revoked = DateTime.UtcNow;

            _context.RefreshTokens.Update(refreshToken);

            await _context.SaveChangesAsync();
        }








        public async Task ForgotPasswordAsync(string email, string origin) {
            var user = await _userManager.FindByEmailAsync(email);
            //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            //    return Ok(new { message = " email da ton tai , link reset da send." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{origin}/api/auth/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendResetPasswordEmail(user.Email, resetLink);
        }






        public async Task ResetPassedAsync(string email, string token, string newPass) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("email k ton tai");
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);

            if (!result.Succeeded) {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Reset password failed: {errors}");
            }

        }

    }
}
