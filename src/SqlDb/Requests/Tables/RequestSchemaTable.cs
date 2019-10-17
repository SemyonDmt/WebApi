using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestSchemaTable
    {
        private readonly string _tblName;
        private readonly string _sql;
        private readonly SqlParameter[] _parameters;

        public RequestSchemaTable(string tblName)
        {
            _tblName = tblName ?? throw new ArgumentNullException(nameof(tblName));
            _sql = BuildSql();
            _parameters = BuildParameters();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            return
                $@"SELECT col.name As Name,
                          col.is_identity As IsKey,
                          types.name As ColumnType,
                          col.is_nullable As IsNull,
                          iif(types.name  = 'nvarchar', CAST(col.max_length / 2 AS varchar(5)), null) AS Size
                  FROM sys.tables tbl
                          LEFT JOIN sys.columns col ON tbl.object_id = col.object_id
                          LEFT JOIN sys.types types ON col.user_type_id = types.user_type_id
                  WHERE tbl.name='{_tblName}'";
        }

        private SqlParameter[] BuildParameters()
        {
            return new[]
            {
                new SqlParameter
                {
                    ParameterName = "@tblName",
                    Value = _tblName
                }
            };
        }
    }
}