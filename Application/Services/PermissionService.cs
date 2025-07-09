using Application.DTOs;
using Application.Interfaces;
using Core.Models.Identity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class PermissionService : IPermissionService {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;



        private readonly string[] predefinedPermissions = new[] { "CanApproveDocument", "CanDeleteViolationDoc", "CanManageTags",
        "CanBanUser", "CanDeleteComment", "CanViewAllUsers"};



        public PermissionService(UserManager<AppUser> userManager, AppDbContext context) {
            _userManager = userManager;
            _context = context;
        }




        public Task<List<string>> GetAllPermissionsAsync() {
            return Task.FromResult(predefinedPermissions.ToList());
        }



        // cach nay load toan bo user theo role nhung k co them thong tin tu bang roles
        public async Task<List<AppUser>> GetAllModsAsync() {
            var mods = await _userManager.GetUsersInRoleAsync("Mod");
            return mods.ToList();
        }

        // linq de lay them data hehe
        public async Task<List<ModInfoDto>> GetAllModsLinqAsync() {
            var query = from r in _context.Roles
                        where r.Name == "Mod"
                        join ur in _context.UserRoles on r.Id equals ur.RoleId
                        join u in _context.Users on ur.UserId equals u.Id
                        select new ModInfoDto {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            Description = r.Description
                        };

            return await query.ToListAsync();
        }


        public async Task<List<string>> GetPermissionsByUserIdAsync(string userId) {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var claims = await _userManager.GetClaimsAsync(user);
            return claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();
        }

        public async Task GrantPermissionAsync(string userId, string permission) {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.Any(c => c.Type == "Permission" && c.Value == permission))
                return;

            await _userManager.AddClaimAsync(user, new Claim("Permission", permission));
        }

        public async Task UpdatePermissionsAsync(string userId, List<string> permissions) {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var currentPermissions = currentClaims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            // Remove old claims
            foreach (var claim in currentClaims.Where(c => c.Type == "Permission")) {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Add new
            foreach (var permission in permissions.Distinct()) {
                await _userManager.AddClaimAsync(user, new Claim("Permission", permission));
            }
        }

        public async Task DeletePermissionAsync(string userId, string permission) {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.RemoveClaimAsync(user, new Claim("Permission", permission));
        }
    }


}

