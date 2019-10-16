using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlDbTests")]
namespace SqlDb.Requests.Tables
{
    internal class RequestSelectTables
    {
        private const string _sql = "SELECT name AS tableName FROM sys.Tables WHERE is_ms_shipped = 'FALSE'";

        public string Sql() => _sql;
    }
}