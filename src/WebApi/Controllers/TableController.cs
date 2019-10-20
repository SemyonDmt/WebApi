using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlDb;
using SqlDb.Commands.Tables;
using SqlDb.Model;
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

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var command = new CommandSelectTables();
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful && command.Data.Length > 0)
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return Content(command.Data, "application/json");
            }

            return NoContent();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var command = new CommandSchemaTable(name);
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Ok(command.Data);

            return NotFound();
        }

        [HttpPost("{name}")]
        public async Task<ActionResult> Create(string name, [FromBody] Column[] tableColumns)
        {
            var command = new CommandCreateTable(name, tableColumns);
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