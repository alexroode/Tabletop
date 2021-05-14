using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabletop.Core.Users;

#nullable enable
namespace Tabletop.Core.Tables
{
    public class TableService
    {
        private readonly Dictionary<string, Table> _tables = new();
        private readonly Dictionary<string, Dictionary<User, TablePlayerMapping>> _playerMapping = new();

        public Table CreateTable(string name, string game)
        {
            var table = new Table()
            {
                Name = name,
                Game = game,
                Id = ShortGuid.NewGuid(),
            };
            _tables[table.Id] = table;
            _playerMapping[table.Id] = new();
            return table;
        }

        public List<Table> GetAllTables()
        {
            return _tables.Values.ToList();
        }

        public Table? GetTable(string id)
        {
            return _tables.GetValueOrDefault(id);
        }

        public void JoinTable(string id, User user)
        {
            var playersAtTable = _playerMapping[id];
            if (!playersAtTable.ContainsKey(user))
            {
                playersAtTable[user] = new()
                {
                    IsHost = playersAtTable.Count == 0,
                    PlayerNumber = 0
                };
            }
        }

        public Dictionary<User, TablePlayerMapping>? GetUsersAtTable(string id)
        {
            return _playerMapping.GetValueOrDefault(id);
        }
    }
}
