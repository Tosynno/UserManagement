using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserDbContext
    {
        private readonly IMongoDatabase _database;
        public UserDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
