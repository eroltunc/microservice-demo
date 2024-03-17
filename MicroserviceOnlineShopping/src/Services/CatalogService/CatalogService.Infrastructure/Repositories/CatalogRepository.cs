using CatalogService.Application.Repositories;
using CatalogService.Domain.AggregateModels.CatalogAggregate;
using CatalogService.Infrastructure.Context;

namespace CatalogService.Infrastructure.Repositories
{
    public class CatalogRepository : GenericRepository<CatalogItem>, ICatalogRepository
    {
        public CatalogRepository(CatalogContext context) : base(context) { }
    }
}
