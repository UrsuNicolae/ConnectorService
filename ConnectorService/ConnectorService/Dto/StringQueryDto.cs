using System.ComponentModel.DataAnnotations;

namespace ConnectorService.Dto
{
    public class StringQueryDto
    {
        [Required]
        public string QueryString { get; set; }
    }
}
