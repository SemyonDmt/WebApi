using System;
using System.Data.SqlClient;
using FluentValidation;
using SqlDb.Commands.Rows;
using SqlDb.Validators;

namespace SqlDb.Requests.Rows
{
    public class RequestCustomSqlQuery
    {
        private readonly string _tableName;
        private readonly string _select;
        private readonly string _order;
        private readonly string _filter;
        private readonly int _page;
        private readonly int _size;
        private readonly string _sql;
        private readonly RequestFormat _format;
        private readonly SqlParameter[] _parameters;
        public string Sql() => _sql;
        public SqlParameter[] Parameters() => _parameters;

        public RequestCustomSqlQuery(string tableName, string @select, string @order, string filter, int page, int size, RequestFormat format)
        {
            new TableNameValidator().ValidateAndThrow(tableName);

            if (!string.IsNullOrEmpty(@select))
                new SelectSqlValidator().ValidateAndThrow(@select);

            if(!string.IsNullOrEmpty(@order))
                new OrderSqlValidator().ValidateAndThrow(@order);

            if(!string.IsNullOrEmpty(filter))
                new WhereSqlValidator().ValidateAndThrow(filter);

            _tableName = tableName;
            _select = @select;
            _order = @order;
            _filter = filter;
            _page = page;
            _size = size;
            _format = format;

            _sql = BuildSql();
            _parameters = BuildParametersInsert();
        }

        private string BuildSql()
        {
            var @select = string.IsNullOrEmpty(_select) ? "*" : _select;
            var @where = string.IsNullOrEmpty(_filter) ? string.Empty : $" WHERE {_filter}";
            var @order = string.IsNullOrEmpty(_order) ? string.Empty : $" ORDER BY {_order}";
            var page = string.Empty;
            string format;

            switch (_format)
            {
                case RequestFormat.Json:
                    format = " FOR JSON AUTO, INCLUDE_NULL_VALUES";
                    break;
                default:
                    format = string.Empty;
                    break;
            }

            if (_page > 0 && _size > 0)
            {
                page = $" OFFSET {(_page - 1) * _size} ROWS FETCH NEXT {_size} ROWS ONLY ";

                if (@order == string.Empty)
                    @order = $" ORDER BY [{_tableName}Id]";
            }

            return $"SELECT {@select} FROM [{_tableName}]{@where}{@order}{page}{format};";
        }

        private SqlParameter[] BuildParametersInsert()
        {
            return Array.Empty<SqlParameter>();
        }
    }
}