using System.Data;
using MediatR;

namespace ConnectorService.Queries
{
    public class GetSqlDataBaseSchemaQuery : GetDbScheme, IRequest<DataTable>
    {
    }
}
