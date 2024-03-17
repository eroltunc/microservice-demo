using CatalogService.Domain.AggregateModels.CatalogAggregate;
using CatalogService.Domain.SeedWork;
using CatalogService.Infrastructure.EntityConfigurations;
using CatalogService.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Context
{
    public class CatalogContext:DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "catalog";
        private readonly IMediator mediator;
       
        public CatalogContext() : base() { }
        public CatalogContext(DbContextOptions<CatalogContext> options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }

        public virtual DbSet<CatalogItem> CatalogItems { get; set; }
        public virtual DbSet<CatalogBrand> CatalogBrands { get; set; }
        public virtual DbSet<CatalogType> CatalogTypes { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            modelBuilder.HasSequence("catalog_brand_hilo").IncrementsBy(10);
            modelBuilder.HasSequence("catalog_hilo").IncrementsBy(10);
            modelBuilder.HasSequence("catalog_type_hilo").IncrementsBy(10);
    
        }
        
    }
 
}
