using AutoMapper;
using ConnectorService.Dto;
using ConnectorService.Queries;

namespace ConnectorService.Profiles
{
    public class DbProfile : Profile
    {
        public DbProfile()
        {
            CreateMap<SchemaDto, GetSqlDataBaseSchemaQuery>();
            CreateMap<SchemaDto, GetPostgresSqlDataBaseSchemaQuery>();
            CreateMap<QueryDto, ExecutePostgresQuery>();
            CreateMap<QueryDto, ExecuteSqlQuery>();
        }
    }
}
