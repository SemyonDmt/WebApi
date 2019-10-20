using System.Runtime.CompilerServices;
using FluentValidation;
using SqlDb.Validators;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestDropTable
    {
        private readonly string _tableName;
        private readonly string _sql;

        public RequestDropTable(string tableName)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            _tableName = tableName;
            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            return
$@"DROP TABLE [{_tableName}];
DROP PROCEDURE IF EXISTS [Insert{_tableName}FromJson];
DROP PROCEDURE IF EXISTS [Update{_tableName}FromJson];
DROP PROCEDURE IF EXISTS [Delete{_tableName}];
DROP TABLE IF EXISTS [{_tableName}Publish];";
        }
    }
}