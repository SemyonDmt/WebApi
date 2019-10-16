using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Rows
{
    internal class RequestUpdate
    {
        private readonly string _tblName;
        private readonly (string colName, object value)[] _cols;
        private readonly int _id;
        private readonly string _sql;
        private readonly SqlParameter[] _parameters;
        private readonly SqlParameter _parameterId;

        public RequestUpdate(string tableName, (string colName, object value)[] cols, int id)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (cols == null || cols.Any(c => string.IsNullOrEmpty(c.colName)))
                throw new ArgumentNullException(nameof(cols));

            _tblName = tableName;
            _cols = cols;
            _id = id;
            _sql = BuildSql();
            _parameterId = BuildParameterId();
            _parameters = BuildParametersInsert();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            var colsName = _cols.Select(s => $"{s.colName}=@{s.colName}").ToArray();
            var sbSet = new StringBuilder().Append(string.Join(", ", colsName));

            return $"UPDATE {_tblName} SET {sbSet} WHERE {_tblName}Id=@{_tblName}Id; SELECT * FROM {_tblName} WHERE {_tblName}Id=@{_tblName}Id FOR JSON PATH";
        }

        private SqlParameter BuildParameterId()
        {
            return new SqlParameter()
            {
                ParameterName = $"@{_tblName}Id",
                Direction = ParameterDirection.Input,
                Value = _id
            };
        }

        private SqlParameter[] BuildParametersInsert()
        {
            var sqlParameters = new SqlParameter[_cols.Length + 1];

            sqlParameters[0] = _parameterId;

            for (var i = 0; i < _cols.Length; i++)
            {
                sqlParameters[i + 1] = new SqlParameter
                {
                    ParameterName = $@"{_cols[i].colName}",
                    Direction = ParameterDirection.Input,
                    Value = _cols[i].value
                };
            }

            return sqlParameters;
        }
    }
}