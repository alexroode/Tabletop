﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Tabletop.Core.Tables
{
    public class TableService
    {
        private readonly Dictionary<string, Table> _tables = new();
        private readonly Dictionary<string, Dictionary<string, int>> _playerMapping = new();

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

        public int? JoinTable(string id, string connectionId)
        {
            var playersAtTable = _playerMapping[id];
            if (!playersAtTable.ContainsKey(connectionId))
            {
                var maxPlayerId = playersAtTable.Any() ? playersAtTable.Max(t => t.Value) : 0;
                playersAtTable[connectionId] = maxPlayerId + 1;
            }

            return playersAtTable[connectionId];
        }
    }
}