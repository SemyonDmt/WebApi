using System.Linq;
using System.Text;
using FluentValidation;
using SqlDb.Model.Structure.Columns;
using SqlDb.Validators;

namespace SqlDb.Requests.Tables
{
    public class RequestCreateProcInsertRow
    {
        private readonly string _tableName;
        private readonly Column[] _cols;
        private readonly string _sql;

        public RequestCreateProcInsertRow(string tableName, Column[] columns)
        {
            new TableNameValidator().ValidateAndThrow(tableName);
            new ColumnArrayValidator().ValidateAndThrow(columns);

            _tableName = tableName;
            _cols = columns;
            _cols = new Column[columns.Length + 1];

            for (var i = 0; i < columns.Length; i++)
                _cols[i] = columns[i];
            _cols[_cols.Length - 1] = new ColumnId($"{tableName}Id");

            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            var col = _cols.Where(w => !(w is ColumnId));
            var sbCols = new StringBuilder().Append(string.Join(", ", col.Select(s => s.Name)));
            var sbCols2 = new StringBuilder().Append(string.Join(", ", col.Select(s => s.SqlRequestInsertColumn())));

            return
$@"CREATE PROCEDURE Insert{_tableName}FromJson(@InsertJson NVARCHAR(MAX), @Id integer OUTPUT)
AS BEGIN
    INSERT INTO {_tableName}({sbCols})
    SELECT {sbCols}
    FROM OPENJSON(@InsertJson)
                WITH ({sbCols2}) as json;

    SET @Id=SCOPE_IDENTITY();
    SELECT * FROM {_tableName} WHERE {_tableName}Id=@Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES;
END";
        }
    }
}