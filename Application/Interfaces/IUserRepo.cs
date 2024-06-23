using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepo
    {
        Task<User> CreateUser(User user);
        Task<long> UpdateUser(User user);
        Task<long> DeactivateUser(string id);
    }
}
