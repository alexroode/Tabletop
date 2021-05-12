using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabletop.Core.Tables;

#nullable enable
namespace Tabletop.Server.Api
{
    [Route("/api/table")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        [Route("{tableid}")]
        [HttpGet]
        public ActionResult<Table> GetTableAsync(string tableId)
        {
            var table = _tableService.GetTable(tableId);

            if (table == null)
            {
                return NotFound();
            }

            return table;
        }
    }
}
