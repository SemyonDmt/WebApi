using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using FluentValidation;
using SqlDb.Validators;

[assembly: InternalsVisibleTo("SqlDbTests")]

namespace SqlDb.Requests.Rows
{
    internal class RequestSelectById
    {
        private readonly string _tblName;
        private readonly int _id;
        private readonly string _sql;
        private readonly SqlParameter[] _parameters;

        public RequestSelectById(string tableName, int id)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            _tblName = tableName;
            _id = id;

            _sql = BuildSql();
            _parameters = BuildParametersInsert();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            return
                $"SELECT * FROM {_tblName} WHERE {_tblName}Id=@id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES";
        }

        private SqlParameter[] BuildParametersInsert()
        {
            return new[]
            {
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Direction = ParameterDirection.Input,
                    Value = _id
                }
            };
        }
    }
}