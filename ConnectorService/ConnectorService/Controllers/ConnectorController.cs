using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Dapper;

namespace ConnectorService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConnectorController : ControllerBase
    {
        [HttpPost]
        public async Task ExecuteQuery([FromBody]string queryString )
        {
           await ExecuteCommand(queryString);
        }

        [HttpGet]
        public async Task<DataTable> GetDbScheme(string connectionString, string? schemaName)
        {
            using var connection = new SqlConnection("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=postgres;Pooling=true;Connection Lifetime=0;");//"Server=localhost;Database=PayHUB;Trusted_Connection=True;");
            await connection.OpenAsync();
            var test = connection.GetSchemaAsync();
            var schema = schemaName is null ? await connection.GetSchemaAsync() : await  connection.GetSchemaAsync(schemaName);
            await connection.CloseAsync();
            return schema;
        }

        private async Task ExecuteCommand(string command)
        {
            using (var connection = new SqlConnection("Server=localhost;Database=PayHUB;Trusted_Connection=True;"))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                {
                    var test = await connection.ExecuteAsync(command);
                    
                    tx.Rollback();
                }
            }
        }
    }
}