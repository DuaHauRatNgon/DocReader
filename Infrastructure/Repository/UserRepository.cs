using Core.Models.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class UserRepository : IUserRepository {
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        public async Task<List<AppUser>> GetAllAsync() {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(string id) {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<string>> GetRolesAsync(AppUser user) {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IList<Claim>> GetClaimsAsync(AppUser user) {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task BlockUserAsync(AppUser user) {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }

        public async Task UnblockUserAsync(AppUser user) {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }

    }
}
