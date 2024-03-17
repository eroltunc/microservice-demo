using CatalogService.Domain.SeedWork;

namespace CatalogService.Domain.AggregateModels.CatalogAggregate
{
    public class CatalogBrand : BaseEntity, IAggregateRoot
    {
        public CatalogBrand():base()
        {
            Id= Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public string Brand { get; set; }
    }
}
