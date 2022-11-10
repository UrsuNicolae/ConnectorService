using System.Data;
using MediatR;

namespace ConnectorService.Queries
{
    public class GetSqlDataBaseSchemaQuery : IRequest<DataTable>
    {
        public string ConnectionString { get; set; }

        public string? CollectionName { get; set; }
    }
}
