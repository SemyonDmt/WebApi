using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SqlDb.Requests.Tables;
using SqlDb.Structure.Columns;

namespace SqlDb.Commands.Tables
{
    public class CommandSchemaTable : ICommand
    {
        private readonly RequestSchemaTable _request;
        public List<Column> Data { get; private set; }

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

                var rows = command.Connection.Query(_request.Sql(), parameters).ToArray();

                if (!rows.Any())
                {
                    Error = @"Table not found";
                    return;
                }

                var cols = new List<Column>();

                foreach (var row in rows)
                {
                    string type = row.type;
                    string name = row.name;
                    bool.TryParse(row.isNull.ToString(), out bool isNull);
                    int size = Convert.ToInt32(row.size);

                    switch (type)
                    {
                        case "int":
                            cols.Add(new ColumnNumeric(name, isNull));
                            break;
                        case "nvarchar":
                            cols.Add(new ColumnString(name, isNull, size));
                            break;
                        case "datetime":
                            cols.Add(new ColumnDatetime(name, isNull));
                            break;
                        case "bit":
                            cols.Add(new ColumnBoolean(name, isNull));
                            break;
                    }
                }

                Data = cols;
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