using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Tables;

namespace SqlDb.Commands.Tables
{
    public class CommandDropTable : ICommand
    {
        private readonly RequestDropTable _request;
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandDropTable(string tableName)
        {
            _request = new RequestDropTable(tableName);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                var result = await command.ExecuteNonQueryAsync(CancellationToken.None);
                if (result == -1)
                {
                    IsSuccessful = true;
                }
                else
                {
                    Error = "Error";
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