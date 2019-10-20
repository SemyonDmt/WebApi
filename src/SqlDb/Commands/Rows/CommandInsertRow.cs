using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandInsertRow : ICommand
    {
        private readonly RequestInsertFromJson _requestJson;
        public string Data { get; private set; }
        public int CreatedId { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandInsertRow(string tableName, string json)
        {
            _requestJson = new RequestInsertFromJson(tableName, json);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _requestJson.Sql();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(_requestJson.Parameters());

                Data = (string) await command.ExecuteScalarAsync(CancellationToken.None);

                if (_requestJson.GetParameterId().Value is int)
                    CreatedId = (int) _requestJson.GetParameterId().Value;

                IsSuccessful = true;
            }
            catch (Exception)
            {
                //Todo Реализовать разновидности ошибок
                Error = "Error";
            }
        }
    }
}