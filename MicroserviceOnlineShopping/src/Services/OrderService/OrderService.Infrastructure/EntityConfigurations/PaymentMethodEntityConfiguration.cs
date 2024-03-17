using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.CustomerAggregate;

namespace OrderService.Persistence.EntityConfigurations
{
    internal class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
        //    paymentConfiguration.ToTable("PaymentMethods", OrderDbContext.DEFAULT_SCHEMA);

            paymentConfiguration.Ignore(b => b.DomainEvents);

            paymentConfiguration.HasKey(o => o.Id);
            paymentConfiguration.Property(i => i.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            paymentConfiguration.Property<int>("CustomerId").IsRequired();

            paymentConfiguration
                .Property(i => i.CardholderName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardHolderName")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration
                .Property(i => i.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Description")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration
                .Property(i => i.CardNumber)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardNumber")
                .HasMaxLength(25)
                .IsRequired();

            paymentConfiguration
                .Property(i => i.ExpirationDate)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ExpirationDate")
                .HasMaxLength(25)
                .IsRequired();

            paymentConfiguration
                .Property(i => i.CardTypeId)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardTypeId")
                .IsRequired();

            paymentConfiguration.HasOne(p => p.CardType)
                .WithMany()
                .HasForeignKey(i => i.CardTypeId);
        }
    }
}
