using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.CustomerAggregate;
using OrderService.Infrastructure.Context;
using OrderService.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Persistence.EntityConfigurations
{
    internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> buyerConfiguration)
        {
            buyerConfiguration.HasKey(b => b.Id);

            buyerConfiguration.Ignore(b => b.DomainEvents);
            buyerConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();
            buyerConfiguration.Property(b => b.Name).HasColumnType("Name").HasColumnType("varchar").HasMaxLength(100);
            buyerConfiguration.HasMany(b => b.PaymentMethods)
               .WithOne()
               .HasForeignKey(i => i.Id)
               .OnDelete(DeleteBehavior.Cascade);
            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Customer.PaymentMethods));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
