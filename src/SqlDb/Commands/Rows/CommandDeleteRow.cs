using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandDeleteRow : ICommand
    {
        private readonly RequestDelete _request;

        public bool IsSuccessful { get; private set; }
        
        public string Error { get; private set; }
        
        public CommandDeleteRow(string tableName, int id)
        {
            _request = new RequestDelete(tableName, id);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
                var result = await command.ExecuteNonQueryAsync(CancellationToken.None);
                if (result> 0)
                {
                    IsSuccessful = true;
                }
                else
                {
                    Error = "Id not found";
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