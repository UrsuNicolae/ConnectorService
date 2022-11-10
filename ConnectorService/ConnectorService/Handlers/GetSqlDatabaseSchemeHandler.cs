using System.Data;
using System.Data.SqlClient;
using ConnectorService.Queries;
using MediatR;

namespace ConnectorService.Handlers
{
    public class GetSqlDatabaseSchemeHandler : IRequestHandler<GetSqlDataBaseSchemaQuery, DataTable>
    {
        public async Task<DataTable> Handle(GetSqlDataBaseSchemaQuery request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(request.ConnectionString); //"User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=postgres;Pooling=true;Connection Lifetime=0;"//"Server=localhost;Database=PayHUB;Trusted_Connection=True;");
            await connection.OpenAsync(cancellationToken);
            var schema = request.CollectionName is null ?
                await connection.GetSchemaAsync(cancellationToken) :
                await connection.GetSchemaAsync(request.CollectionName, cancellationToken);
            await connection.CloseAsync();
            return schema;
        }
    }
}
