using System.Data;
using MediatR;

namespace ConnectorService.Queries
{
    public class GetPostgresSqlDataBaseSchemaQuery : IRequest<DataTable>
    {
        public string ConnectionString { get; set; }

        public string? CollectionName { get; set; }
    }
}
