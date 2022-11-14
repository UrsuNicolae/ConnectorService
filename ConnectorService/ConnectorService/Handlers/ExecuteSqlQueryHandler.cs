using ConnectorService.Queries;
using MediatR;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ConnectorService.Handlers
{
    public class ExecuteSqlQueryHandler : IRequestHandler<ExecuteSqlQuery, object>
    {
        public async Task<object> Handle(ExecuteSqlQuery request, CancellationToken cancellationToken)
        {
            if (request.QueryString.Contains("Select", StringComparison.CurrentCultureIgnoreCase))
            {
                return await ExecuteSelectAsync(request);
            }
            return await ExecuteQueryAsync(request);
        }

        private async Task<List<Dictionary<string, object>>> ExecuteSelectAsync(ExecuteSqlQuery request)
        {
            using (SqlConnection conn = new SqlConnection(request.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(request.QueryString, conn))
            {
                conn.Open();
                var reader = await cmd.ExecuteReaderAsync();
                var result = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
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

        private async Task<int> ExecuteQueryAsync(ExecuteSqlQuery request)
        {
            using (SqlConnection conn = new SqlConnection(request.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(request.QueryString, conn))
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
