using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandUpdateRow : ICommand
    {
        private readonly RequestUpdateFromJson _requestJson;

        public string Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandUpdateRow(string tableName, int id, string json)
        {
            _requestJson = new RequestUpdateFromJson(tableName, id, json);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _requestJson.Sql();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(_requestJson.Parameters());
                var result = (string) await command.ExecuteScalarAsync(CancellationToken.None);
                if (string.IsNullOrEmpty(result))
                {
                    Error = "Id not found";
                }
                else
                {
                    Data = result;
                    IsSuccessful = true;
                }
            }
            catch (Exception)
            {
                //Todo Реализовать разновидности ошибок
                Error = "Error";
            }
        }
    }
}