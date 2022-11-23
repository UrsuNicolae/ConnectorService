using ConnectorService.Queries;
using MediatR;
using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DbType = ConnectorService.Models.Enums.DbType;

namespace ConnectorService.Handlers
{
    public class GetPostgresSqlDatabaseSchemeHandler : IRequestHandler<GetDbSchemeQuery, DataTable>
    {
        public async Task<DataTable> Handle(GetDbSchemeQuery request, CancellationToken cancellationToken)
        {
            switch (request.DataBaseType)
            {
                case DbType.MySql:
                    return await GetSchemeAsync<SqlConnection>(request, cancellationToken);
                case DbType.Postgres:
                    return await GetSchemeAsync<NpgsqlConnection>(request, cancellationToken);
                case DbType.SQLite:
                    return await GetSchemeAsync<SqliteConnection > (request, cancellationToken);
                default: throw new NotSupportedException("Unsupported database");
            }
        }

        public async Task<DataTable> GetSchemeAsync<Connection>(GetDbSchemeQuery request, CancellationToken cancellationToken) where Connection : DbConnection
        {
            await using Connection connection = (Connection)Activator.CreateInstance(typeof(Connection), new object[] { request.ConnectionString });
            await connection.OpenAsync(cancellationToken);
            var schema = request.CollectionName is null ?
                await connection.GetSchemaAsync(cancellationToken) :
                await connection.GetSchemaAsync(request.CollectionName, cancellationToken);
            await connection.CloseAsync();
            return schema;
        }
    }
}
