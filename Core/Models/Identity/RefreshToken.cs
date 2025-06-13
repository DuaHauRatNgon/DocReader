using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity {
    public class RefreshToken {

        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }          // FK đến AppUser
        public AppUser User { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Expires { get; set; }


        public DateTime? Revoked { get; set; }


        public bool IsRevoked => Revoked != null;
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsRevoked && !IsExpired;

    }
}
