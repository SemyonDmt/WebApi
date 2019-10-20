using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Tables;

namespace SqlDb.Commands.Tables
{
    public class CommandSelectTables : ICommand
    {
        private readonly RequestSelectTables _request;
        public string Data { get; private set; }

        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandSelectTables()
        {
            _request = new RequestSelectTables();
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
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