using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SqlDb.Commands;

namespace SqlDb
{
    public class DbCon
    {
        private readonly string _connectionString;

        public DbCon(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public async Task ExecuteAsync(ICommand command)
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                await connection.OpenAsync();
                await command.ExecuteAsync(cmd);
            }
        }
    }
}