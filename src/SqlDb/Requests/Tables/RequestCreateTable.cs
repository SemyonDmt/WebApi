using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FluentValidation;
using SqlDb.Model.Structure.Columns;
using SqlDb.Validators;

[assembly: InternalsVisibleTo("SqlDbTests")]

namespace SqlDb.Requests.Tables
{
    internal class RequestCreateTable
    {
        private readonly string _tableName;
        private readonly Column[] _cols;
        private readonly string _sql;

        public RequestCreateTable(string tableName, Column[] columns)
        {
            new TableNameValidator().ValidateAndThrow(tableName);
            new ColumnArrayValidator().ValidateAndThrow(columns);

            _tableName = tableName;
            _cols = new Column[columns.Length + 1];

            for (var i = 0; i < columns.Length; i++)
                _cols[i] = columns[i];
            _cols[_cols.Length - 1] = new ColumnId($"{tableName}Id");

            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            var sbCols = new StringBuilder().Append(string.Join(", ", _cols.Select(s => s.SqlRequestCreateColumn())));
            return $@"CREATE TABLE {_tableName} ({sbCols})";
        }
    }
}