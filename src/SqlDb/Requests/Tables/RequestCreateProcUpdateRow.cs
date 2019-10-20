using System.Linq;
using System.Text;
using FluentValidation;
using SqlDb.Model.Structure.Columns;
using SqlDb.Validators;

namespace SqlDb.Requests.Tables
{
    public class RequestCreateProcUpdateRow
    {
        private readonly string _tableName;
        private readonly Column[] _cols;
        private readonly string _sql;

        public RequestCreateProcUpdateRow(string tableName, Column[] columns)
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
            var sbCols = new StringBuilder().Append(string.Join(", ", col.Select(s => $"{s.Name} = json.{s.Name}")));
            var sbCols2 = new StringBuilder().Append(string.Join(", ", col.Select(s => s.SqlRequestInsertColumn())));

            return
$@"CREATE PROCEDURE Update{_tableName}FromJson(@Id int, @UpdateJson NVARCHAR(MAX))
AS BEGIN

UPDATE {_tableName} SET {sbCols}
FROM OPENJSON(@UpdateJson)
              WITH ({sbCols2}) as json
WHERE {_tableName}Id = @Id;

SELECT * FROM {_tableName} WHERE {_tableName}Id=@Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
END";
        }
    }
}