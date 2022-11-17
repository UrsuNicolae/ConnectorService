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
                result.data = await _mediator.Send(_mapper.Map<ExecuteQuery>(queryDto));
                return result;
            }
            catch (Exception e)
            {
                return new ErrorModel(false, e.Message);
            }
        }

        [HttpPost]
        public async Task<object> VerifyDbConnection([FromBody] ConnectionDto connectionDto)
        {
            try
            {
                var result = new SuccessModel(true, "Connection Verified")
                {
                    data = await _mediator.Send(_mapper.Map<VerifyConnectionStringQuery>(connectionDto))
                };
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
                result.data = await _mediator.Send(_mapper.Map<GetDbSchemeQuery>(schemaDto));

                return result;
            }
            catch (Exception e)
            {
                return new ErrorModel(false, e.Message);
            }
        }
    }
}