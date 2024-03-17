using CatalogService.Domain.AggregateModels.CatalogAggregate;

namespace CatalogService.Application.Models.Dto.Catalog
{
    public class CatalogItemDto
    {
        public Guid Id { get; set; }
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
