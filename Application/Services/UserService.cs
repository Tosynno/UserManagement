using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userService;

        public UserService(IUserRepo userService)
        {
            _userService = userService;
        }

        public async Task<string> CreateUser(UserRequest user)
        {
            var res = await _userService.GetAllUsersAsync();
            var chk = res.FirstOrDefault(c => c.Email == user.Email);
            if (chk != null)
            {
                return "User already exist";
            }
            else
            {
                User sa = new User();
                sa.Id = Guid.NewGuid().ToString();
                sa.Username = user.Username;
                sa.Email = user.Email;
                sa.Password = user.Password;
                sa.ReferenceId = Guid.NewGuid().ToString();
                sa.IsActive = true;
                sa.CreatedDate = DateTime.Now;
                var result = await _userService.CreateUser(sa);
                if (result != null)
                {
                    return "User created successful";
                }
                else
                {
                    return "unable to create user !!!";
                }
            }
            
        }

        public async Task<string> DeactivateUser(string ReferenceId)
        {
            var result = await _userService.DeactivateUser(ReferenceId);
            if (result == 0)
            {
                return "User successfully deactivated";
            }
            else
            {
                return "Unable to deactivated user !!!";
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var Allresponse = new List<UserDto>();
            var result = await _userService.GetAllUsersAsync();
            foreach (var item in result)
            {
                var response = new UserDto(item);
                Allresponse.Add(response);
            }
            return Allresponse;
        }

        public async Task<UserDto> GetUserByIdAsync(string ReferenceId)
        {
            var result = await _userService.GetUserByIdAsync(ReferenceId);
            var response = new UserDto(result);
            return response;
        }

        public async Task<string> UpdateUser(UpdateUserRequest user)
        {
            User sa = new User();
            sa.Username = user.Username;
            sa.Email = user.Email;
            sa.Password = user.Password;
            sa.ReferenceId = user.ReferenceId;
            sa.ModifyDate = DateTime.Now;
            var result = await _userService.UpdateUser(sa);
            if (result == 0)
            {
                return "User Updated successfully";
            }
            else
            {
                return "Unable to update user !!!";
            }
        }
    }
}
