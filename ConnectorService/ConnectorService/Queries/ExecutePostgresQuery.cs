using MediatR;

namespace ConnectorService.Queries
{
    public class ExecutePostgresQuery :ExecuteQuery, IRequest<object>
    {
    }
}
