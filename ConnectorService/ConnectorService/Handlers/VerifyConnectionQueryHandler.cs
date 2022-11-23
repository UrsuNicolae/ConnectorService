using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ConnectorService.Queries;
using MediatR;
using Microsoft.Data.Sqlite;
using Npgsql;
using DbType = ConnectorService.Models.Enums.DbType;

namespace ConnectorService.Handlers
{
    public class VerifyConnectionQueryHandler : IRequestHandler<VerifyConnectionStringQuery, bool>
    {
        public async Task<bool> Handle(VerifyConnectionStringQuery request, CancellationToken cancellationToken)
        {
            switch (request.DataBaseType)
            {
                case DbType.MySql:
                    return await VerifyPostgresConnectionAsync<SqlConnection>(request.ConnectionString);
                case DbType.Postgres:
                    return await VerifyPostgresConnectionAsync<NpgsqlConnection>(request.ConnectionString);
                case DbType.SQLite:
                    return await VerifyPostgresConnectionAsync<SqliteConnection > (request.ConnectionString);
                default: throw new NotSupportedException("Unsupported database");
            }
        }

        private async Task<bool> VerifyPostgresConnectionAsync<T>(string ConnectionString) where T : DbConnection
        {
            await using T conn = (T)Activator.CreateInstance(typeof(T), new object[] { ConnectionString });
            await conn.OpenAsync();
            if (conn.State == ConnectionState.Open)
            {
                await conn.CloseAsync();
                return true;
            }

            return false;
        }
    
}
}
