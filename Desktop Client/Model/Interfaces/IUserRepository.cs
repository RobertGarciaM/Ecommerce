using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> CreateUserAsync(CreateUserDto user);
        Task<bool> UpdateUserAsync(string userId, UserDto user);
        Task<bool> DeleteUserAsync(string userId);
    }

}
