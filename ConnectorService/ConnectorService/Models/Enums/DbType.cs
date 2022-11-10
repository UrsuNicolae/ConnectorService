using System.ComponentModel;

namespace ConnectorService.Models.Enums
{
    public enum DbType
    {
        [Description("MySql")]
        MySql = 0,

        [Description("Postgres")]
        Postgres = 1
    }
}
