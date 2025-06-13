using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity {
    public class AppUser : IdentityUser {
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmailConfirmed => EmailConfirmed;
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
