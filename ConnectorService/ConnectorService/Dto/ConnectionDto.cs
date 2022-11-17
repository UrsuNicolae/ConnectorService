using System.ComponentModel.DataAnnotations;
using ConnectorService.Models.Enums;

namespace ConnectorService.Dto
{
    public class ConnectionDto
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public DbType DataBaseType { get; set; }
    }
}
