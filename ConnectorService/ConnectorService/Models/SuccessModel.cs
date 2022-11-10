namespace ConnectorService.Models
{
    public class SuccessModel
    {

        public SuccessModel(bool success, string message) : base()
        {
            this.success = success;
            this.message = message;
        }

        public SuccessModel()
        {
        }

        public bool success { get; set; }

        public object data { get; set; }

        public string message { get; set; }

    }
}
