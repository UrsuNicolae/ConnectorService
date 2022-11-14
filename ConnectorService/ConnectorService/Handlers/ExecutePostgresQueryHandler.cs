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
                return await ExecuteSelectAsync(request);
            }
            return await ExecuteQueryAsync(request);
        }

        private async Task<List<Dictionary<string, object>>> ExecuteSelectAsync(ExecutePostgresQuery request)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(request.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(request.QueryString, conn))
            {
                conn.Open();
                var reader = await cmd.ExecuteReaderAsync();
                var result = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    result.Add(row);
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
