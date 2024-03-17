using CatalogService.Domain.AggregateModels.CatalogAggregate;
using CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.EntityConfigurations
{
    public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> entity)
        {
            entity.ToTable("CatalogBrand", CatalogContext.DEFAULT_SCHEMA);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(x => x.Brand).IsRequired().HasMaxLength(100);
        }
    }
}
