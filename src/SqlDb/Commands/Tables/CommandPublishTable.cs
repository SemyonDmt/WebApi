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
        private readonly string _tableName;
        public string Data { get; private set; }

        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandPublishTable(string tableName)
        {
            _request = new RequestPublishTable(tableName);
            _tableName = tableName;
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                await command.ExecuteNonQueryAsync(CancellationToken.None);
                Data = $"{_tableName}Publish";
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