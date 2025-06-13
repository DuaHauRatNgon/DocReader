using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models.Identity;


namespace Infrastructure.Identity {
    public class RoleSeeder {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<RoleSeeder> _logger;

        public RoleSeeder(RoleManager<AppRole> roleManager, ILogger<RoleSeeder> logger) {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedRolesAsync() {
            string[] roles = { "Admin", "Mod", "Author", "User" };

            foreach (var role in roles) {
                if (!await _roleManager.RoleExistsAsync(role)) {
                    var result = await _roleManager.CreateAsync(new AppRole(role) {
                        Id = Guid.NewGuid().ToString(),
                        Description = $"{role} role",
                        IsSystemRole = true
                    });
                    if (result.Succeeded) {
                        _logger.LogInformation($"Role '{role}' created.");
                    }
                    else {
                        _logger.LogWarning($"Failed to create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

    }
}