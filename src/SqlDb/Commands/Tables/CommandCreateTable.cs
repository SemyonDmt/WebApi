using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Requests.Tables;
using SqlDb.Structure.Columns;

namespace SqlDb.Commands.Tables
{
    public class CommandCreateTable: ICommand
    {
        private readonly RequestCreateTable _request;
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandCreateTable(string tableName,  Column[] columns)
        {
            _request = new RequestCreateTable(tableName, columns);
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