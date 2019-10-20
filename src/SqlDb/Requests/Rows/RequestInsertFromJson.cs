using System;
using System.Data;
using System.Data.SqlClient;
using FluentValidation;
using SqlDb.Validators;

namespace SqlDb.Requests.Rows
{
    public class RequestInsertFromJson
    {
        private readonly string _tblName;
        private readonly string _json;

        private readonly string _sql;
        private readonly SqlParameter[] _parameters;
        private readonly SqlParameter _parameterId;

        public RequestInsertFromJson(string tableName, string json)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(tableName));

            _tblName = tableName;
            _json = json;
            _sql = BuildSql();
            _parameterId = BuildParameterId();
            _parameters = BuildParametersInsert();
        }

        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        public SqlParameter GetParameterId() => _parameterId;
        private string BuildSql()
        {
            return $@"Insert{_tblName}FromJson";
        }

        private SqlParameter BuildParameterId()
        {
            return new SqlParameter()
            {
                ParameterName = $"@Id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
        }

        private SqlParameter[] BuildParametersInsert()
        {
            return new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InsertJson",
                    Direction = ParameterDirection.Input,
                    Value = _json
                },
                _parameterId
            };
        }
    }
}