using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity {
    public interface IUserRepository {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(string id);
        Task<List<string>> GetRolesAsync(AppUser user);
        Task<IList<Claim>> GetClaimsAsync(AppUser user);
        Task BlockUserAsync(AppUser user);
        Task UnblockUserAsync(AppUser user);

    }
}
