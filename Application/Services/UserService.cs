using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync() {
            var users = await _userRepository.GetAllAsync();
            var result = new List<UserDto>();

            foreach (var user in users) {
                var roles = await _userRepository.GetRolesAsync(user);
                result.Add(new UserDto {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    IsBlocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow,
                    Roles = roles
                });
            }

            return result;
        }

        public async Task<UserDetailDto> GetUserByIdAsync(string id) {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            var roles = await _userRepository.GetRolesAsync(user);
            var claims = await _userRepository.GetClaimsAsync(user);

            return new UserDetailDto {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Avatar = user.Avatar,
                IsBlocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow,
                Roles = roles,
                Claims = claims.Select(c => new ClaimDto {
                    Type = c.Type,
                    Value = c.Value
                }).ToList()
            };
        }

        public async Task BlockUserAsync(string id) {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            await _userRepository.BlockUserAsync(user);
        }

        public async Task UnblockUserAsync(string id) {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            await _userRepository.UnblockUserAsync(user);
        }
    }
}
