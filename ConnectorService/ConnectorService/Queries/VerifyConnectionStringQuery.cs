using ConnectorService.Models.Enums;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace ConnectorService.Queries
{
    public class VerifyConnectionStringQuery : IRequest<bool>
    {
        public string ConnectionString { get; set; }
        
        public DbType DataBaseType { get; set; }
    }
}
