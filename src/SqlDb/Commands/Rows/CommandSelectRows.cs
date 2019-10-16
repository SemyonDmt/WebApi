using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandSelectRows : ICommand
    {
        private readonly RequestSelect _request;
        public string Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandSelectRows(string tableName, (string colName, object value)[] cols)
        {
            _request = new RequestSelect(tableName, cols);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
                Data = (string) await command.ExecuteScalarAsync(CancellationToken.None);
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