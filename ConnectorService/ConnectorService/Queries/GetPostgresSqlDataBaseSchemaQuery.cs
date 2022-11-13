using System.Data;
using MediatR;

namespace ConnectorService.Queries
{
    public class GetPostgresSqlDataBaseSchemaQuery : GetDbScheme, IRequest<DataTable>
    {
    }
}
