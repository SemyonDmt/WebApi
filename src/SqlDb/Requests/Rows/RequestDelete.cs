using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using FluentValidation;
using SqlDb.Validators;

[assembly: InternalsVisibleTo("SqlDbTests")]

namespace SqlDb.Requests.Rows
{
    internal class RequestDelete
    {
        private readonly string _tblName;
        private readonly int _id;
        private readonly string _sql;
        private readonly SqlParameter[] _parameters;

        public RequestDelete(string tableName, int id)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            _tblName = tableName;
            _id = id;
            _sql = BuildSql();
            _parameters = BuildParameterId();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            return $"Delete{_tblName}";
        }

        private SqlParameter[] BuildParameterId()
        {
            return new[]
            {
                new SqlParameter()
                {
                    ParameterName = $"@Id",
                    Direction = ParameterDirection.Input,
                    Value = _id
                }
            };
        }
    }
}