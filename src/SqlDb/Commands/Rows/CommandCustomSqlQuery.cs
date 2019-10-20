using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandCustomSqlQuery : ICommand
    {
        private readonly RequestCustomSqlQuery _request;
        public string Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandCustomSqlQuery(string tableName, string select, string order, string filter, int page, int size)
        {
            _request = new RequestCustomSqlQuery(tableName, select, order, filter, page, size, RequestFormat.Json);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
                var data = (string) await command.ExecuteScalarAsync(CancellationToken.None);

                Data = data ?? string.Empty;
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