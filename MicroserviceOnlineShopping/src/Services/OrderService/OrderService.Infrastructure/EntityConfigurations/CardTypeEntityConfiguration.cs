using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.CustomerAggregate;

namespace OrderService.Persistence.EntityConfigurations
{
    internal class CardTypeEntityConfiguration : IEntityTypeConfiguration<CardType>
    {
        public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
        {
          //  cardTypesConfiguration.ToTable("CardTypes", OrderDbContext.DEFAULT_SCHEMA);

            cardTypesConfiguration.HasKey(ct => ct.Id);
            cardTypesConfiguration.Property(i => i.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            cardTypesConfiguration.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            cardTypesConfiguration.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
