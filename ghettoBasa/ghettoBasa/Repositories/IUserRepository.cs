using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetUsers();
        IEnumerable<Users> GetAdminUsers();
        IEnumerable<Users> GetClientUsers();
        Users GetUser(string UserId);
        Users GetUserByUsername(string Username);
        Users GetUserByIdentity(string Natid);
        void CreateUser(Users user);
        bool UpdateUser(Users user);
        IEnumerable<Users> SearchUsers(string Parameter);
        IEnumerable<Users> GetDeletedUsers();
        bool UserDeleteStatus(string UserId, bool action);
        bool UserStatus(string UserId, string action);
    }
}
