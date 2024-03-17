using CatalogService.Domain.SeedWork;
namespace CatalogService.Domain.AggregateModels.CatalogAggregate
{
    public class CatalogType: BaseEntity,IAggregateRoot
    {
        public CatalogType() : base()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public string Type { get; set; }
    }
}
