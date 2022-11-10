using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using AutoMapper;
using ConnectorService.Dto;
using ConnectorService.Models;
using ConnectorService.Queries;
using Dapper;
using MediatR;
using DbType = ConnectorService.Models.Enums.DbType;

namespace ConnectorService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConnectorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ConnectorController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task ExecuteQuery([FromBody]string queryString )
        {
           await ExecuteCommand(queryString);
        }

        [HttpPost]
        public async Task<object> DbScheme(SchemaDto schemaDto)
        {
            try
            {
                var result = new SuccessModel(true, "Schema retrieved");
                switch (schemaDto.DataBaseType)
                {
                    case DbType.MySql:
                        result.data = await _mediator.Send(_mapper.Map<GetSqlDataBaseSchemaQuery>(schemaDto));
                        break;
                    case DbType.Postgres:
                        result.data = await _mediator.Send(_mapper.Map<GetPostgresSqlDataBaseSchemaQuery>(schemaDto));
                        break;
                    default:
                        throw new ArgumentException("Unsupported DbType");
                }

                return result;
            }
            catch (Exception e)
            {
                return new ErrorModel(false, e.Message);
            }
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