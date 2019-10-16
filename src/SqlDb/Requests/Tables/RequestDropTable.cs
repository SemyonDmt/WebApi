using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestDropTable
    {
        private readonly string _tblName;
        private readonly string _sql;

        public RequestDropTable(string tblName)
        {
            _tblName = tblName ?? throw new ArgumentNullException(nameof(tblName));
            _sql = BuildSqlRequestCreateTable();
        }

        public string Sql() => _sql;

        private string BuildSqlRequestCreateTable()
        {
            return $"DROP TABLE {_tblName}; DROP TABLE IF EXISTS {_tblName}Publish;";
        }
    }
}