using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces {
    public interface IUserService {

        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDetailDto> GetUserByIdAsync(string id);
        Task BlockUserAsync(string id);
        Task UnblockUserAsync(string id);
    }
}

