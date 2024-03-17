using Consul;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;

using IdentityService.Business.IntegrationEvents.Events;
using IdentityService.Business.Services;
using IdentityService.DataAccess.Contexts;
using IdentityService.Entity.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService.Business
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddConsulServiceRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(_configuration["ConsulConfiguration:ConsulAddress"]);
            }));
            return _services;
        }
        public static IServiceCollection AddAuthenticationServiceRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IdentityDbContext>();
            _services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });
            _services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AuthenticationConfiguration:SecretKey"]))
                };
            });
            return _services;
        }
        public static IServiceCollection AddIntegrationEventServiceRegistration(this IServiceCollection _services)
        {
            _services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "IdentityService",
                    EventBusType = EventBusType.RabbitMQ,
                };
                return EventBusFactory.Create(config, sp);
            });
            return _services;
        }
        public static IServiceCollection AddBusinessRegistration(this IServiceCollection _services, IConfiguration configuration)
        {

          
            _services.AddScoped<IAuthService, AuthService>();
            _services.AddSingleton<ITokenService, TokenService>();
            _services.AddHttpContextAccessor();
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>().UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            using (var dbContext = new IdentityDbContext(optionsBuilder.Options))
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
           
            return _services;
        }
        public static IApplicationBuilder AddConsulServiceRegistrationBuilder(this IApplicationBuilder _applicationBuilder, IHostApplicationLifetime _hostApplicationLifetime, IConfiguration _configuration)
        {
            var consulClient = _applicationBuilder.ApplicationServices.GetRequiredService<IConsulClient>();
            var uri = _configuration.GetValue<Uri>("ConsulConfiguration:ServiceAddress");
            var serviceName = _configuration.GetValue<string>("ConsulConfiguration:ServiceName");
            var serviceId = _configuration.GetValue<string>("ConsulConfiguration:ServiceId");

            var registration = new AgentServiceRegistration()
            {
                ID = serviceId ?? "UndefinedService" + new Guid(),
                Name = serviceName ?? "UndefinedService" + new Guid(),
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { serviceName, serviceId }
            };
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();
            _hostApplicationLifetime.ApplicationStopping.Register(() => consulClient.Agent.ServiceDeregister(registration.ID).Wait());
            return _applicationBuilder;
        }
        public static IApplicationBuilder AddIntegrationEventServiceRegistrationBuilder(this IApplicationBuilder _applicationBuilder)
        {
            IEventBus eventBus = _applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            return _applicationBuilder;
        }
    }
}
