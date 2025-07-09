using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity {
    public class IdentityDataSeeder {
        public static async Task SeedAsync(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager) {
            string[] roles = new[] { "User", "Author", "Mod", "Admin" };

            foreach (var roleName in roles) {
                if (!await roleManager.RoleExistsAsync(roleName)) {
                    var role = new AppRole {
                        Name = roleName,
                        Description = $"Default {roleName} role",
                        IsSystemRole = true
                    };
                    await roleManager.CreateAsync(role);
                }
            }





            //seed
            var permissionClaims = new[]
            {
            "CanApproveDocument",
            "CanDeleteViolationDoc",
            "CanManageTags",
            "CanBanUser",
            "CanDeleteComment",
            "CanViewAllUsers"
        };

            var mod1Email = "mod1@yahoo.com";
            var mod2Email = "mod2@yahoo.com";
            var adminEmail = "admin@duck.com";







            var mod1 = await CreateUserIfNotExists(userManager, mod1Email, "Mod@123", "Mod_1");
            await userManager.AddToRoleAsync(mod1, "Mod");

            await AddClaimIfNotExists(userManager, mod1, "Permission", "CanApproveDocument");
            await AddClaimIfNotExists(userManager, mod1, "Permission", "CanManageTags");
            await AddClaimIfNotExists(userManager, mod1, "Permission", "CanDeleteViolationDoc");







            var mod2 = await CreateUserIfNotExists(userManager, mod2Email, "Mod@123", "Mod_2");
            await userManager.AddToRoleAsync(mod2, "Mod");

            await AddClaimIfNotExists(userManager, mod2, "Permission", "CanBanUser");
            await AddClaimIfNotExists(userManager, mod2, "Permission", "CanDeleteComment");
            await AddClaimIfNotExists(userManager, mod2, "Permission", "CanViewAllUsers");







            var admin = await CreateUserIfNotExists(userManager, adminEmail, "Admin@123", "Super Admin");
            await userManager.AddToRoleAsync(admin, "Admin");

            foreach (var claim in permissionClaims) {
                await AddClaimIfNotExists(userManager, admin, "Permission", claim);
            }






            var user = await CreateUserIfNotExists(userManager, "user@gmail.com", "User@123", "User thong thuong");
            await userManager.AddToRoleAsync(user, "User");

            var author = await CreateUserIfNotExists(userManager, "author@gmail.com", "Author@123", "Content Author");
            await userManager.AddToRoleAsync(author, "Author");
        }







        private static async Task<AppUser> CreateUserIfNotExists(
            UserManager<AppUser> userManager,
            string email, string password, string fullName) {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) {
                user = new AppUser {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, password);
            }
            return user;
        }

        private static async Task AddClaimIfNotExists(
            UserManager<AppUser> userManager,
            AppUser user,
            string type,
            string value) {
            var claims = await userManager.GetClaimsAsync(user);
            if (!claims.Any(c => c.Type == type && c.Value == value)) {
                await userManager.AddClaimAsync(user, new Claim(type, value));
            }
        }
    }

}
