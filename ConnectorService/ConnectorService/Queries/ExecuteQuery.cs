using System.ComponentModel.DataAnnotations;

namespace ConnectorService.Queries
{
    public class ExecuteQuery
    {
        public string ConnectionString { get; set; }
        public string QueryString { get; set; }
    }
}
