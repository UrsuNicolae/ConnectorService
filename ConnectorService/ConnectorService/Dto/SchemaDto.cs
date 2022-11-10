using System.ComponentModel.DataAnnotations;
using ConnectorService.Models.Enums;

namespace ConnectorService.Dto
{
    public class SchemaDto
    {
        [Required]
        public string ConnectionString { get; set; }

        public string? CollectionName { get; set; }

        [Required]
        public DbType DataBaseType { get; set; }
    }
}
