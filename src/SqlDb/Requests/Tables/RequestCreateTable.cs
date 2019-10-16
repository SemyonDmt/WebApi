using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SqlDb.Structure.Columns;

[assembly: InternalsVisibleTo("SqlDbTests")]

namespace SqlDb.Requests.Tables
{
    internal class RequestCreateTable
    {
        private readonly string _tblName;
        private readonly Column[] _cols;
        private readonly string _sql;

        public RequestCreateTable(string name, Column[] columns)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _tblName = name;
            _cols = columns ?? throw new ArgumentNullException(nameof(columns));
            _cols = new Column[columns.Length + 1];

            for (var i = 0; i < columns.Length; i++)
                _cols[i] = columns[i];
            _cols[_cols.Length - 1] = new ColumnId($"{name}Id");

            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            var sbCols = new StringBuilder().Append(string.Join(", ", _cols.Select(s => s.SqlRequestCreateColumn())));
            return $"CREATE TABLE {_tblName} ({sbCols});";
        }
    }
}