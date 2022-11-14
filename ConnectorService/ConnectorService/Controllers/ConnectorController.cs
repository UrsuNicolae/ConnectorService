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
        public async Task<object> ExecuteQuery([FromBody]QueryDto queryDto)
        {
            try
            {
                var result = new SuccessModel(true, "Query Executed");
                switch (queryDto.DataBaseType)
                {
                    case DbType.MySql:
                        result.data = await _mediator.Send(_mapper.Map<ExecuteSqlQuery>(queryDto));
                        break;
                    case DbType.Postgres:
                        result.data = await _mediator.Send(_mapper.Map<ExecutePostgresQuery>(queryDto));
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

        [HttpPost]
        public async Task<object> DbScheme([FromBody]SchemaDto schemaDto)
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
    }
}