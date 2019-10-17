using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SqlDb.Model;
using SqlDb.Requests.Tables;

namespace SqlDb.Commands.Tables
{
    public class CommandSchemaTable : ICommand
    {
        private readonly RequestSchemaTable _request;
        public Column[] Data { get; private set; }

        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }


        public CommandSchemaTable(string tableName)
        {
            _request = new RequestSchemaTable(tableName);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                var parameters = new DynamicParameters();

                Array.ForEach(_request.Parameters(), parameter =>
                    parameters.Add(name: parameter.ParameterName, value: parameter.Value));

                var rows = (await command.Connection.QueryAsync<Column>(_request.Sql(), parameters)).ToArray();

                if (!rows.Any())
                {
                    Error = @"Table not found";
                    return;
                }

                Data = rows;
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