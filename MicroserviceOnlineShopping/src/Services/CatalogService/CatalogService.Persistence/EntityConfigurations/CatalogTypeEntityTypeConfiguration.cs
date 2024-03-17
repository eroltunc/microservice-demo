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
    public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> entity)
        {
            entity.ToTable("CatalogType", CatalogContext.DEFAULT_SCHEMA);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");      
            entity.Property(x => x.Type).IsRequired().HasMaxLength(100);
        }
    }
}
