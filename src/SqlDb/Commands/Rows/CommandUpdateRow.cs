using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandUpdateRow : ICommand
    {
        private readonly RequestUpdate _request;

        public string Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandUpdateRow(string tableName, (string colName, object value)[] cols, int id)
        {
            _request = new RequestUpdate(tableName, cols, id);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
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