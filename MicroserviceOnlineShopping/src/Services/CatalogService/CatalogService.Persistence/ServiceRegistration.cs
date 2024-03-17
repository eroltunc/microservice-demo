using CatalogService.Application.Repositories;
using CatalogService.Persistence.Context;
using CatalogService.Persistence.Extensions;
using CatalogService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CatalogService.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            services.AddScoped<ICatalogRepository, CatalogRepository>();
       
        }
    }
}
