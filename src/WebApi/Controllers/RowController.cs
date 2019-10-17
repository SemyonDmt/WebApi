using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlDb;
using SqlDb.Commands.Rows;
using SqlDb.Commands.Tables;
using SqlDb.Model;
using SqlDb.Structure.Columns;
using Column = SqlDb.Model.Column;

namespace WebApi.Controllers
{
    [Route("api/[controller]/Table")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly DbCon _dbCon;

        public RowController(DbCon dbCon)
        {
            _dbCon = dbCon ?? throw new ArgumentNullException(nameof(dbCon));
        }

//        // GET
//        [HttpGet]
//        public ActionResult<IEnumerable<string>> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

        // GET
        [HttpGet("{name}/{id}")]
        public async Task<ActionResult> GetById(string name, int id)
        {
            var command = new CommandSelectRows(name, new (string colName, object value)[]{($"{name}Id", id)});
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Ok(command.Data);

            return NotFound();
        }

        // POST api/values
        [HttpPost("{name}")]
        public async Task<ActionResult> Create(string name, [FromBody]  Row newValue)
        {

            var command = new CommandInsertRow(name, newValue.Items.Select(s => (s.Name, s.ObjectValue)).ToArray());
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Created($"api/Row/{name}/{command.CreatedId}", command.Data);

            return Conflict(command.Error);
        }

//        // PUT api/values/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }
//
//        // DELETE api/values/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
    }
}