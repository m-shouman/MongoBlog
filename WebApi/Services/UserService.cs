using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IMongoBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usersCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<User> Create(string username)
        {
            var user = new User { Username = username, CreatedOn = DateTime.Now };
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<List<User>> Get()
        {
            return await (await _usersCollection.FindAsync(user => true)).ToListAsync();
        }

        public async Task<User> Get(string id)
        {
            return await (await _usersCollection.FindAsync(user => user.Id == id)).FirstOrDefaultAsync();
        }

        public async Task Update(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(user => user.Id == id, updatedUser);
        }

        public Task Remove(string id)
        {
            return _usersCollection.DeleteOneAsync(user => user.Id == id);
        }
    }
}
