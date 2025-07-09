using Application.DTOs;
using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces {
    public interface IPermissionService {
        Task<List<string>> GetAllPermissionsAsync();
        Task<List<AppUser>> GetAllModsAsync();
        Task<List<ModInfoDto>> GetAllModsLinqAsync();
        Task<List<string>> GetPermissionsByUserIdAsync(string userId);
        Task GrantPermissionAsync(string userId, string permission);
        Task UpdatePermissionsAsync(string userId, List<string> permissions);
        Task DeletePermissionAsync(string userId, string permission);
    }
}
