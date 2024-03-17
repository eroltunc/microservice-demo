using CatalogService.Domain.AggregateModels.CatalogAggregate;

namespace CatalogService.Application.Repositories
{
    public interface ICatalogRepository : IGenericRepository<CatalogItem>
    {
    }
}
