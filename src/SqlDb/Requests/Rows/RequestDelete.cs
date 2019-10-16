using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Rows
{
    internal class RequestDelete
    {
        private readonly string _tblName;
        private readonly int _id;
        private readonly string _sql;
        private readonly SqlParameter _parameterId;

        public RequestDelete(string tableName, int id)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            _tblName = tableName;
            _id = id;
            _sql = BuildSql();
            _parameterId = BuildParameterId();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => new []{_parameterId};
        
        private string BuildSql()
        {
            return $"DELETE FROM {_tblName} WHERE {_tblName}Id=@{_tblName}id;";
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
    }
}