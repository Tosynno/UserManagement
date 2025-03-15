using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class UserRepo : IUserRepo    
    {
        private readonly UserDbContext _context;

        public UserRepo(UserDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
            return user;
        }

        public async Task<long> DeactivateUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var update = Builders<User>.Update.Set("IsActive", false);

            var result = await _context.Users.UpdateOneAsync(filter, update);
            return result.MatchedCount;
        }

        public async Task<User> GetUserByIdAsync(string ReferenceId)
        {
            var filter = Builders<User>.Filter.Eq("ReferenceId", ReferenceId);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Find(_ => true).ToListAsync();
        }

        public async Task<long> UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq("ReferenceId", user.ReferenceId);
            var update = Builders<User>.Update
                .Set("Username", user.Username)
                .Set("Email", user.Email)
                .Set("IsActive", user.IsActive)
                .Set("Password", user.Password)
                .Set("CreatedDate", user.CreatedDate);

            var result = await _context.Users.UpdateOneAsync(filter, update);
            return result.MatchedCount;
        }
    }
}
