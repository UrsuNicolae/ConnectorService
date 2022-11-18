using AutoMapper;
using ConnectorService.Dto;
using ConnectorService.Queries;

namespace ConnectorService.Profiles
{
    public class DbProfile : Profile
    {
        public DbProfile()
        {
            CreateMap<ConnectionDto, VerifyConnectionStringQuery>();
            CreateMap<SchemaDto, GetDbSchemeQuery>();
            CreateMap<QueryDto, ExecuteQuery>();
            CreateMap<StringQueryDto, StringQuery>();
        }
    }
}
