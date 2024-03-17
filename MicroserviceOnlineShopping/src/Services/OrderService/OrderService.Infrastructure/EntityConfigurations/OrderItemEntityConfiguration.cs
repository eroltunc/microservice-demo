using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;

namespace OrderService.Persistence.EntityConfigurations
{
    class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
        {
         //   orderItemConfiguration.ToTable("OrderItems", OrderDbContext.DEFAULT_SCHEMA);

            orderItemConfiguration.HasKey(o => o.Id);
            orderItemConfiguration.Property(o => o.Id).ValueGeneratedOnAdd();
            orderItemConfiguration.Ignore(b => b.DomainEvents);
           // orderItemConfiguration.Property(o=>o.ProductId).ValueGeneratedOnAdd();

           

          //  orderItemConfiguration.Property<Guid>("OrderId").IsRequired();
        }
    }
}
