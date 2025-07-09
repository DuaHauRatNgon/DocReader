using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Interfaces;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity {
    public class JwtTokenGenerator : ITokenGenerator {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;



        public JwtTokenGenerator(IConfiguration config, UserManager<AppUser> userManager) {
            _config = config;
            _userManager = userManager;
        }




        public async Task<string> GenerateAccessTokenAsync(AppUser user) {
            var authClaims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles) {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            authClaims.AddRange(userClaims);

            var tokenKey = _config["Jwt:Key"] ?? throw new Exception("JWT Key k tim thay");
            var tokenIssuer = _config["Jwt:Issuer"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenIssuer,
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string Token, DateTime Expires)> GenerateRefreshTokenAsync(string userId) {
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expiresAt = DateTime.UtcNow.AddDays(7);
            return (refreshToken, expiresAt);
        }
    }
}
