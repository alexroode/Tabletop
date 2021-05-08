using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabletop.Core.Chat
{
    public record User
    {
        public string Id { get; init; }
        public string DisplayName { get; init; }
    }
}
