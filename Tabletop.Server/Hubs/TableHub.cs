using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core.Tables;
using Tabletop.Core.Users;

namespace Tabletop.Server.Hubs
{
    public interface ITableClient
    {
        Task UpdateUsers(Dictionary<User, TablePlayerMapping> users);
    }

    public class TableHub : Hub<ITableClient>
    {
        private readonly TableService _tableService;
        private readonly UserService _userService;

        public TableHub(TableService tableService, UserService userService)
        {
            _tableService = tableService;
            _userService = userService;
        }

        public async Task JoinTable(string tableId)
        {
            var user = _userService.GetOrAddUser(Context.User);

            _tableService.JoinTable(tableId, user);
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);

            var tableUsers = _tableService.GetUsersAtTable(tableId);
            await Clients.Group(tableId).UpdateUsers(tableUsers);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
