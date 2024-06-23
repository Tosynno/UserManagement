using Application.Dtos;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUser(UserRequest user);
        Task<string> UpdateUser(UpdateUserRequest user);
        Task<string> DeactivateUser(string id);
        Task<UserDto> GetUserByIdAsync(string ReferenceId);
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
