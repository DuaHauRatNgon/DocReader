using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces {
    public interface ITokenGenerator {
        Task<string> GenerateAccessTokenAsync(AppUser user);
        Task<(string Token, DateTime Expires)> GenerateRefreshTokenAsync(string userId);
    }

}
