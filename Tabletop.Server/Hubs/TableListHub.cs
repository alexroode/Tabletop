using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core.Tables;

namespace Tabletop.Server.Hubs
{
    public interface ITableListClient
    {
        Task OwnTableCreated();
        Task JoinTable(Table table, int playerAssignment);
        Task TableCreated(Table table);
        Task UpdateTables(List<Table> table);
        Task TableRemoved(Table table);
    }

    public class TableListHub : Hub<ITableListClient>
    {
        private readonly TableService _tableService;

        public TableListHub(TableService tableService)
        {
            _tableService = tableService;
        }

        public async Task CreateTable(string name, string game)
        {
            var table = _tableService.CreateTable(name, game);
            await Clients.All.TableCreated(table);

            await Clients.Caller.OwnTableCreated();

            /*var playerAssignment = _tableService.JoinTable(table.Id, Context.ConnectionId);
            if (playerAssignment != null)
            {
                await Clients.Caller.JoinTable(table, playerAssignment.Value);
            }*/
        }

        public override async Task OnConnectedAsync()
        {
            var tables = _tableService.GetAllTables();
            await Clients.Caller.UpdateTables(tables);
            await base.OnConnectedAsync();
        }
    }
}
