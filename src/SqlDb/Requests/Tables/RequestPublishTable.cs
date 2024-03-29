using System.Runtime.CompilerServices;
using FluentValidation;
using SqlDb.Validators;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestPublishTable
    {
        private readonly string _tableName;
        private readonly string _sql;

        public RequestPublishTable(string tableName)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            _tableName = tableName;
            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            return $"DROP TABLE IF EXISTS {_tableName}Publish; SELECT * INTO {_tableName}Publish FROM  {_tableName};";
        }
    }
}