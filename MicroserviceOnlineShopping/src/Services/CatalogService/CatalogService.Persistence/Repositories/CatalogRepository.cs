using CatalogService.Application.Repositories;
using CatalogService.Domain.AggregateModels.CatalogAggregate;
using CatalogService.Persistence.Context;

namespace CatalogService.Persistence.Repositories
{
    public class CatalogRepository : GenericRepository<CatalogItem>, ICatalogRepository
    {
        public CatalogRepository(CatalogContext context) : base(context) { }
    }
}
