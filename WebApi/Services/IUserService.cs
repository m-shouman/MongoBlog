using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<User> Create(string username);
        Task<List<User>> Get();
        Task<User> Get(string id);
        Task Remove(string id);
        Task Update(string id, User updatedUser);
    }
}