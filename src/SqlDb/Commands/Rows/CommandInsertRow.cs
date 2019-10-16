using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandInsertRow : ICommand
    {
        private readonly RequestInsert _request;
        public string Data { get; private set; }
        public int CreatedId { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandInsertRow(string tableName, (string colName, object value)[] cols)
        {
            _request = new RequestInsert(tableName, cols);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());
                Data = (string) await command.ExecuteScalarAsync(CancellationToken.None);
                if (_request.GetParameterId().Value is int)
                    CreatedId = (int) _request.GetParameterId().Value;

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