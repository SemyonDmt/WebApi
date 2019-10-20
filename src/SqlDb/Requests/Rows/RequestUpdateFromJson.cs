using System;
using System.Data;
using System.Data.SqlClient;
using FluentValidation;
using SqlDb.Validators;

namespace SqlDb.Requests.Rows
{
    public class RequestUpdateFromJson
    {
        private readonly string _tblName;
        private readonly string _json;
        private readonly int _id;

        private readonly string _sql;
        private readonly SqlParameter[] _parameters;

        public RequestUpdateFromJson(string tableName, int id, string json)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(tableName));

            _tblName = tableName;
            _json = json;
            _id = id;
            _sql = BuildSql();
            _parameters = BuildParametersInsert();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        private string BuildSql()
        {
            return $@"Update{_tblName}FromJson";
        }

        private SqlParameter[] BuildParametersInsert()
        {
            return new[]
            {
                new SqlParameter()
                {
                    ParameterName = $"@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = _id
                },
                new SqlParameter
                {
                    ParameterName = "@UpdateJson",
                    Direction = ParameterDirection.Input,
                    Value = _json
                }
            };
        }
    }
}