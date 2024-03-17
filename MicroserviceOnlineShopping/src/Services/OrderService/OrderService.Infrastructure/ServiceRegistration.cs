using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infrastructure.Context;
using OrderService.Persistence.Context;
using OrderService.Persistence.Repositories;

namespace OrderService.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using (var dbContext = new OrderDbContext(optionsBuilder.Options, null))
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
               new OrderDbContextSeed().SeedAsync(dbContext).Wait();
            }



            return services;
        }
    }
}
