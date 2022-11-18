using MediatR;

namespace ConnectorService.Queries
{
    public class StringQuery : IRequest<string>
    {
        public string QueryString { get; set; }
    }
}
