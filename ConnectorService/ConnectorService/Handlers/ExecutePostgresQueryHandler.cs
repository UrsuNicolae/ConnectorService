using ConnectorService.Queries;
using MediatR;
using Npgsql;

namespace ConnectorService.Handlers
{
    public class ExecutePostgresQueryHandler : IRequestHandler<ExecutePostgresQuery, object>
    {
        public async Task<object> Handle(ExecutePostgresQuery request, CancellationToken cancellationToken)
        {
            if (request.QueryString.Contains("Select", StringComparison.CurrentCultureIgnoreCase))
            {
                return ExecuteSelectAsync(request);
            }
            return ExecuteQueryAsync(request);
        }

        private async Task<string> ExecuteSelectAsync(ExecutePostgresQuery request)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(request.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(request.QueryString, conn))
            {
                conn.Open();
                var reader = await cmd.ExecuteReaderAsync();
                var columns = await reader.GetColumnSchemaAsync();
                var result = string.Empty;
                foreach (var column in columns)
                {
                    result += column.ColumnName + "\t";
                }
                result += "\n";

                while (reader.Read())
                {
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        result += reader.GetValue(i) + "\t";
                    }
                    result += "\n";
                    }
                reader.Close();
                cmd.Dispose();
                conn.Close();
                return result;
            }
        }

        private async Task<int> ExecuteQueryAsync(ExecutePostgresQuery request)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(request.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(request.QueryString, conn))
            {
                conn.Open();
                var rowsAfected = await cmd.ExecuteNonQueryAsync();

                cmd.Dispose();
                conn.Close();
                return rowsAfected;
            }
        }
    }
}
