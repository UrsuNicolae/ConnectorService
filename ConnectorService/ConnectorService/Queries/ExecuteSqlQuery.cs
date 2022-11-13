using MediatR;

namespace ConnectorService.Queries
{
    public class ExecuteSqlQuery : ExecuteQuery, IRequest<object>
    {
    }
}
