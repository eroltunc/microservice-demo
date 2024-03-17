using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.CustomerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.EntityConfigurations;
using OrderService.Infrastructure.Extensions;
using OrderService.Persistence.EntityConfigurations;

namespace OrderService.Infrastructure.Context
{
    public class OrderDbContext: DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "order";
        private readonly IMediator mediator;

        public OrderDbContext(): base() {   }

        public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator): base(options)
        {
            this.mediator = mediator;
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityConfiguration());
        }
    }
}
