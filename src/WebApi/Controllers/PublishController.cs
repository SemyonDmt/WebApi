using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlDb;
using SqlDb.Commands.Tables;

namespace WebApi.Controllers
{
    [Route("api/Table/{name}/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly DbCon _dbCon;

        public PublishController(DbCon dbCon)
        {
            _dbCon = dbCon ?? throw new ArgumentNullException(nameof(dbCon));
        }

        [HttpPost]
        public async Task<ActionResult> Publish(string name)
        {
            var command = new CommandPublishTable(name);
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return Created($"api/Table/{name}Publish", command.Data);

            return NotFound(command.Error);
        }
    }
}