using CatalogService.Domain.SeedWork;
namespace CatalogService.Domain.AggregateModels.CatalogAggregate
{
    public class CatalogItem : BaseEntity,IAggregateRoot
    {
        public CatalogItem() : base()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureFileName { get; set; }

        public string PictureUri { get; set; }

        public int AvailableStock { get; set; }

        public bool OnReorder { get; set; }

        public Guid CatalogTypeId { get; set; }

        public CatalogType CatalogType { get; set; }

        public Guid CatalogBrandId { get; set; }

        public CatalogBrand CatalogBrand { get; set; }

    }
}
