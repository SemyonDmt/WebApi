using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlDb;
using SqlDb.Commands.Tables;
using SqlDb.Model;
using SqlDb.Structure.Columns;
using Column = SqlDb.Model.Column;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly DbCon _dbCon;

        public TableController(DbCon dbCon)
        {
            _dbCon = dbCon ?? throw new ArgumentNullException(nameof(dbCon));
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var command = new CommandSelectTables();
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Ok(command.Data);

            return NoContent();
        }

        // GET
        [HttpGet("{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var command = new CommandSchemaTable(name);
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Ok(command.Data);

            return NotFound();
        }

        // POST
        [HttpPost("{name}")]
        public async Task<ActionResult> Create(string name, [FromBody] Column[] tableColumns)
        {
            var list = new List<SqlDb.Structure.Columns.Column>();

            foreach (var tableColumn in tableColumns)
            {
                switch (tableColumn.ColumnType)
                {
                    case ColumnType.none:
                        break;
                    case ColumnType.@int:
                        list.Add(new ColumnNumeric(tableColumn.Name, tableColumn.IsNull));
                        break;
                    case ColumnType.nvarchar:
                        list.Add(new ColumnString(tableColumn.Name, tableColumn.IsNull, tableColumn.Size ?? 0));
                        break;
                    case ColumnType.datetime:
                        list.Add(new ColumnDatetime(tableColumn.Name, tableColumn.IsNull));
                        break;
                    case ColumnType.bit:
                        list.Add(new ColumnBoolean(tableColumn.Name, tableColumn.IsNull));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var command = new CommandCreateTable(name, list.ToArray());
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
            {
                var cmd = new CommandSchemaTable(name);
                await _dbCon.ExecuteAsync(cmd);
                if (cmd.IsSuccessful)
                    return Created($"api/Table/{name}", cmd.Data);
            }

            return Conflict(command.Error);
        }

        // DELETE
        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            var command = new CommandDropTable(name);
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return NoContent();

            return NotFound();
        }
    }
}