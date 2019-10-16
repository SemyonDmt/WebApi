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
                "SELECT COLUMN_NAME AS name, DATA_TYPE AS type, IIF(IS_NULLABLE='YES', 'TRUE', 'FALSE') AS isNull, CHARACTER_MAXIMUM_LENGTH AS size FROM INFORMATION_SCHEMA.COLUMNS WHERE [table_name]=@tblName;";
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