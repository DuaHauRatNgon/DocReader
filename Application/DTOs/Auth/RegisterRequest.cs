﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth {
    public class RegisterRequest {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }

}
