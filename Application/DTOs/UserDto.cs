using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs {
    public class UserDto {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
