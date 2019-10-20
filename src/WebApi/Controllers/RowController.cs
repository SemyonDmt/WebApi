using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlDb;
using SqlDb.Commands.Rows;

namespace WebApi.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/Table/{name}/[controller]")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly DbCon _dbCon;

        public RowController(DbCon dbCon)
        {
            _dbCon = dbCon ?? throw new ArgumentNullException(nameof(dbCon));
        }

        /// <summary>
        /// Get rows from table
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <param name="select">Select sql</param>
        /// <param name="order">Order sql</param>
        /// <param name="filter">Where sql</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get(
            string name,
            [FromQuery(Name = "$page")] int page,
            [FromQuery(Name = "$size")] int size,
            [FromQuery(Name = "$select")] string select,
            [FromQuery(Name = "$order")] string order,
            [FromQuery(Name = "$filter")] string filter)
        {
            var command = new CommandCustomSqlQuery(name, select, order, filter, page, size);
            await _dbCon.ExecuteAsync(command);

            if (!command.IsSuccessful)
                return NotFound();

            Response.StatusCode = StatusCodes.Status200OK;
            return Content(command.Data, "application/json");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string name, int id)
        {
            var command = new CommandSelectRowsById(name, id);
            await _dbCon.ExecuteAsync(command);

            if (!command.IsSuccessful)
                return NotFound();

            Response.StatusCode = StatusCodes.Status200OK;
            return Content(command.Data, "application/json");
        }

        /// <summary>
        /// Create row
        /// </summary>
        /// <param name="name"></param>
        /// <param name="json"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "Name": "Test",
        ///        "Age": 11,
        ///        "Date": "2019-10-18T19:04:30",
        ///        "IsTest": true
        ///     }
        ///
        /// Sample response:
        ///
        ///     {
        ///        "Name": "Test",
        ///        "Age": 11,
        ///        "Date": "2019-10-18T19:04:30",
        ///        "IsTest": true,
        ///        "OrdersId": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(string name, [FromBody] JsonElement json)
        {
            var command = new CommandInsertRow(name, json.ToString());
            await _dbCon.ExecuteAsync(command);

            if (!command.IsSuccessful)
                return BadRequest(command.Error);

            Response.StatusCode = StatusCodes.Status201Created;
            Response.Headers.Add("Location", $"api/Table/{name}/Row/{command.CreatedId}");
            return Content(command.Data, "application/json");
        }

        /// <summary>
        /// Update row
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT
        ///     {
        ///        "Name": "Test",
        ///        "Age": 11,
        ///        "Date": "2019-10-18T19:04:30",
        ///        "IsTest": false
        ///     }
        ///
        /// Sample response:
        ///
        ///     {
        ///        "Name": "Test",
        ///        "Age": 11,
        ///        "Date": "2019-10-18T19:04:30",
        ///        "IsTest": false,
        ///        "OrdersId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string name, int id, [FromBody] JsonElement newValue)
        {
            var command = new CommandUpdateRow(name, id, newValue.ToString());
            await _dbCon.ExecuteAsync(command);

            if (!command.IsSuccessful)
                return NotFound();

            Response.StatusCode = StatusCodes.Status200OK;
            return Content(command.Data, "application/json");
        }

        /// <summary>
        /// Delete row
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string name, int id)
        {
            var command = new CommandDeleteRow(name, id);
            await _dbCon.ExecuteAsync(command);

            if (command.IsSuccessful)
                return NoContent();

            return NotFound();
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="select"></param>
        /// <param name="order"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("Download/export.xlsx")]
        public async Task<ActionResult> Download(
            string name,
            [FromQuery(Name = "$page")] int page,
            [FromQuery(Name = "$size")] int size,
            [FromQuery(Name = "$select")] string select,
            [FromQuery(Name = "$order")] string order,
            [FromQuery(Name = "$filter")] string filter)
        {
            var command = new CommandCustomSqlQueryToExcel(name, select, order, filter, page, size);
            await _dbCon.ExecuteAsync(command);

            if (!command.IsSuccessful)
                return NotFound();

            if (command.Data.Length == 0)
                return NoContent();

            return File(
                fileContents: command.Data,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "export.xlsx"
            );
        }
    }
}