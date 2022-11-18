using System.ComponentModel.DataAnnotations;
using ConnectorService.Models.Enums;
using MediatR;

namespace ConnectorService.Queries
{
    public class ExecuteQuery: IRequest<object>
    {
        public string ConnectionString { get; set; }
        public string QueryString { get; set; }
        public DbType DataBaseType { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
