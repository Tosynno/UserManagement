using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
                
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReferenceId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public UserDto(User user)
        {
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;   
            ReferenceId = user.ReferenceId; 
            IsActive = user.IsActive;
            CreatedDate = user.CreatedDate;
            ModifyDate = user.ModifyDate;
        }
    }
}
