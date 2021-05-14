using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Tabletop.Core.Users
{
    public record User
    {
        public string Id { get; init; }
        public string DisplayName { get; init; }

        public User() { }

        public User(ClaimsPrincipal claimsPrincipal) 
        {
            Id = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            DisplayName = claimsPrincipal.Identity.Name;
        }
    }
}