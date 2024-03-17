using CatalogService.Application.Repositories;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Extensions;
using CatalogService.Infrastructure.Repositories;
using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CatalogService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddDbContext<CatalogContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            _services.AddScoped<ICatalogRepository, CatalogRepository>();
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            using (var dbContext = new CatalogContext(optionsBuilder.Options,null))
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
                new CatalogContextSeed().SeedAsync(dbContext).Wait();
            }
            return _services;
       
        }

      
    }
}
