using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestPublishTable
    {
        private readonly string _tblName;
        private readonly string _sql;

        public RequestPublishTable(string tblName)
        {
            _tblName = tblName ?? throw new ArgumentNullException(nameof(tblName));
            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            return $"DROP TABLE IF EXISTS {_tblName}Publish; SELECT * INTO {_tblName}Publish FROM  {_tblName};";
        }
    }
}