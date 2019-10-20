using FluentValidation;
using SqlDb.Validators;

namespace SqlDb.Requests.Tables
{
    public class RequestCreateProcDeleteRow
    {
        private readonly string _tableName;
        private readonly string _sql;

        public RequestCreateProcDeleteRow(string tableName)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            _tableName = tableName;
            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            return
$@"CREATE PROCEDURE Delete{_tableName}(@Id int)
AS BEGIN
DELETE FROM {_tableName} WHERE {_tableName}Id=@Id;
END";
        }
    }
}