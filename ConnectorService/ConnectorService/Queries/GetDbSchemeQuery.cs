using System.Data;
using MediatR;
using DbType = ConnectorService.Models.Enums.DbType;

namespace ConnectorService.Queries
{
    public class GetDbSchemeQuery: IRequest<DataTable>
    {
        public string ConnectionString { get; set; }

        public string? CollectionName { get; set; }

        public DbType DataBaseType { get; set; }
    }
}
