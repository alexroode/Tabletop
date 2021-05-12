using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core.Tables;

namespace Tabletop.Server.Hubs
{
    public interface ITableClient
    {
        Task UserJoined(string name);
    }

    public class TableHub : Hub<ITableClient>
    {
        private readonly TableService _tableService;

        public TableHub(TableService tableService)
        {
            _tableService = tableService;
        }

        public async Task JoinTable(string tableId)
        {
            _tableService.JoinTable(tableId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
            await Clients.Group(tableId).UserJoined(Context.ConnectionId);
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
