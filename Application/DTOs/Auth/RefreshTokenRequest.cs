﻿using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth {
    public class RefreshTokenRequest {
        public string RefreshToken { get; set; }
    }

}
