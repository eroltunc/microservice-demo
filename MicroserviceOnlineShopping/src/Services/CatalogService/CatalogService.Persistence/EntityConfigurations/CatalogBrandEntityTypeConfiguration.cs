using CatalogService.Persistence.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogService.Domain.AggregateModels.CatalogAggregate;

namespace CatalogService.Persistence.EntityConfigurations
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
