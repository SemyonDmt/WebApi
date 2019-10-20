using System;
using System.Linq;

namespace SqlDb.Validators
{
    public static class SqlInjection
    {
        private static readonly string[] SqlCheckList = new[]
        {
            "--", ";--", ";", "/*", "*/", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast",
            "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill",
            "select", "sys", "sysobjects", "syscolumns", "table", "update"
        };

        public static bool CheckForSqlInjection(this string sql)
        {
            var checkString = sql.Replace("'", "''");
            return SqlCheckList.Any(checkList => checkString.IndexOf(checkList, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}