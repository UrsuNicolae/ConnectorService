﻿using System.ComponentModel;

namespace ConnectorService.Models.Enums
{
    public enum DbType
    {
        [Description("MySql")]
        MySql = 0       ,

        [Description("Postgres")]
        Postgres = 1,
        
        [Description("SQLite")]
        SQLite = 2,
        
        [Description("Oracle")]
        Oracle = 3,


    }
}
