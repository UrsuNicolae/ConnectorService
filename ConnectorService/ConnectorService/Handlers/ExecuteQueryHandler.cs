using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using ConnectorService.Helpers;
using ConnectorService.Models.Enums;
using ConnectorService.Queries;
using MediatR;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace ConnectorService.Handlers
{
    public class ExecuteQueryHandler : IRequestHandler<ExecuteQuery, object>
    {
        public async Task<object> Handle(ExecuteQuery request, CancellationToken cancellationToken)
        {
            switch (request.DataBaseType)
            {
                case DbType.MySql:
                    if (request.QueryString.Contains("Select", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return await ExecuteSelectAsync<SqlConnection, SqlCommand>(request);
                    }
                    return await ExecuteQueryAsync<SqlConnection, SqlCommand>(request);
                case DbType.Postgres:
                    if (request.QueryString.Contains("Select", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return await ExecuteSelectAsync<NpgsqlConnection, NpgsqlCommand>(request);
                    }
                    return await ExecuteQueryAsync<NpgsqlConnection, NpgsqlCommand>(request);
                case DbType.Oracle:
                    if (request.QueryString.Contains("Select", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return await ExecuteSelectAsync<OracleConnection, OracleCommand>(request);
                    }
                    return await ExecuteQueryAsync<OracleConnection, OracleCommand>(request);
                default: throw new NotSupportedException("Unsupported database");
            }
            
        }

        private async Task<object> ExecuteSelectAsync<Connection, Command>(ExecuteQuery request)
            where Connection: DbConnection
            where Command: DbCommand
        {
            await using Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), new object[] { request.ConnectionString });
            await using Command cmd =
                (Command)Activator.CreateInstance(typeof(Command), new object[] { request.QueryString, conn });
            {
                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                var result = new List<Dictionary<string, object>>();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        if (!await reader.IsDBNullAsync(i))
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        else
                        {
                            row.Add(reader.GetName(i), null);
                        }
                    }
                    result.Add(row);
                }
                await reader.CloseAsync();
                await cmd.DisposeAsync();
                await conn.CloseAsync();
                var currentPageResult = PagedList<Dictionary<string, object>>
                    .ToPagedList(result, request.PageNumber, request.PageSize);

                return new
                {
                    Results = currentPageResult,
                    TotalCount = currentPageResult.TotalCount,
                    CurrentPage = currentPageResult.CurrentPage,
                    TotalPages = currentPageResult.TotalPages,
                    PageSize = currentPageResult.PageSize
                };
            }
        }

        private async Task<string> ExecuteQueryAsync<Connection, Command>(ExecuteQuery request)
            where Connection : DbConnection
            where Command : DbCommand
        {
            await using Connection conn = (Connection)Activator.CreateInstance(typeof(Connection), new object[] { request.ConnectionString });
            await using Command cmd = (Command)Activator.CreateInstance(typeof(Command), new object[] { request.QueryString, conn });
            {
                await conn.OpenAsync();
                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                cmd.Dispose();
                await conn.CloseAsync();
                return "Rows affected: " + rowsAffected;
            }
        }
    }
}
