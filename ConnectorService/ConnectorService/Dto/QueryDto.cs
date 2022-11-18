using System.ComponentModel.DataAnnotations;
using ConnectorService.Models.Enums;

namespace ConnectorService.Dto
{
    public class QueryDto
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string QueryString { get; set; }

        [Required]
        public DbType DataBaseType { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
