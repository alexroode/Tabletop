using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Tabletop.Core.Users
{
    public class UserService
    {
        private readonly Dictionary<string, User> _users = new();

        public User GetOrAddUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = new User(claimsPrincipal);
            if (!_users.ContainsKey(user.Id))
            {
                _users[user.Id] = user;
            }

            return _users[user.Id];
        }

        public User? GetUser(string id)
        {
            return _users.GetValueOrDefault(id);
        }
    }
}
