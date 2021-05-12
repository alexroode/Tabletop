using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Tabletop.Core.Tables
{
    public class ClientTableService
    {
        private readonly Dictionary<string, Table> _cachedTables = new();

        private readonly HttpClient _httpClient;

        public ClientTableService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(Constants.PublicApiClientName);
        }

        public void CacheTable(Table table)
        {
            _cachedTables[table.Id] = table;
        }

        public void CacheTables(IEnumerable<Table> tables)
        {
            foreach (var table in tables)
            {
                CacheTable(table);
            }
        }

        public async Task<Table?> GetTableAsync(string tableId)
        {
            try
            {
                var table = _cachedTables.GetValueOrDefault(tableId);
                if (table != null)
                {
                    return table;
                }

                table = await _httpClient.GetFromJsonAsync<Table>($"api/table/{tableId}");
                if (table != null)
                {
                    _cachedTables[tableId] = table;
                }

                return table;
            }
            catch
            {
                return null;
            }
        }
    }
}
