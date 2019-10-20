using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandSelectRowsById : ICommand
    {
        private readonly RequestSelectById _request;
        public string Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandSelectRowsById(string tableName, int id)
        {
            _request = new RequestSelectById(tableName, id);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
                var data = (string) await command.ExecuteScalarAsync(CancellationToken.None);

                if (data.Length > 0)
                {
                    Data = data;
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