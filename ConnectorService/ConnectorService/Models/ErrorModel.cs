namespace ConnectorService.Models
{
    public class ErrorModel
    {

        public ErrorModel(bool success, string error) : base()
        {
            this.error = error;
            this.success = success;
        }
        public ErrorModel()
        {
            
        }
        public bool success { get; set; }

        public string error { get; set; }
    }
}
