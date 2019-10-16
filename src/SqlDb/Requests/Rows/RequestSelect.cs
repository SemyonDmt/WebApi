using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Rows
{
    internal class RequestSelect
    {
        private readonly string _tblName;
        private readonly (string colName, object value)[] _cols;
        private readonly string _sql;
        private readonly SqlParameter[] _parameters;

        public RequestSelect(string tableName, (string colName, object value)[] cols)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (cols == null || cols.Any(c => string.IsNullOrEmpty(c.colName)))
                throw new ArgumentNullException(nameof(cols));

            _tblName = tableName;
            _cols = cols;
            _sql = BuildSql();
            _parameters = BuildParametersInsert();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            var colsName = _cols.Select(s => $"{s.colName}=@{s.colName}").ToArray();
            var sbWhere = new StringBuilder().Append(string.Join(" AND ", colsName));
            
            return $"SELECT * FROM {_tblName} WHERE {sbWhere} FOR JSON PATH";
        }

        private SqlParameter[] BuildParametersInsert()
        {
            var sqlParameters = new SqlParameter[_cols.Length];
            
            for (var i = 0; i < _cols.Length; i++)
            {
                sqlParameters[i] = new SqlParameter
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