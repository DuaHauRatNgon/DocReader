using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity {
    public class AppRole : IdentityRole<string> {

        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) {
        }

        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
    }
}
