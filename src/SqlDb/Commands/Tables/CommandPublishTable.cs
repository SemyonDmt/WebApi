using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Tables;

namespace SqlDb.Commands.Tables
{
    public class CommandPublishTable : ICommand
    {
        private readonly RequestPublishTable _request;
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandPublishTable(string tableName)
        {
            _request = new RequestPublishTable(tableName);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                await command.ExecuteNonQueryAsync(CancellationToken.None);
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