using CatalogService.Domain.AggregateModels.CatalogAggregate;
using CatalogService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Persistence.EntityConfigurations
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> entity)
        {
            entity.ToTable("Catalog", CatalogContext.DEFAULT_SCHEMA);
            entity.HasIndex(e => e.CatalogBrandId, "IX_Catalog_CatalogBrandId");
            entity.HasIndex(e => e.CatalogTypeId, "IX_Catalog_CatalogTypeId");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(ci => ci.Name).IsRequired(true).HasMaxLength(50);
            entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(18, 2)");
            entity.Property(x => x.PictureFileName).IsRequired(false);
            entity.Ignore(x => x.PictureUri);
        }
    }
}
