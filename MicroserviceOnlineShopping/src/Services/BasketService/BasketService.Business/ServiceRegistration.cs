using BasketService.Application.IntegrationEvents.EventHandlers;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Business.Mapping;
using BasketService.Business.Services.Abstract;
using BasketService.Business.Services.Concrete;
using BasketService.DataAccess.Abstract;
using BasketService.DataAccess.Concrate;
using Consul;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace BasketService.Business
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
                    SubscriberClientAppName = "BasketService",
                    EventBusType = EventBusType.RabbitMQ,
                };
                return EventBusFactory.Create(config, sp);
            });
            _services.AddTransient<OrderCreatedIntegrationEventHandler>();
            return _services;
        }
        public static IServiceCollection AddBusinessRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {          
            _services.AddTransient<IBasketDal, BasketDal>();
            _services.AddTransient<IIdentityService, IdentityService>();
            _services.AddTransient<IBasketService, BasketService.Business.Services.Concrete.BasketService>();
            //AutoMapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            _services.AddSingleton(mapper);
            return _services;
        }
        public static IServiceCollection AddRedisServiceRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddSingleton(sp => sp.ConfigureRedis(_configuration));    
            return _services;
        }
        public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider _services, IConfiguration _configuration)
        {
            var redisConf = ConfigurationOptions.Parse(_configuration.GetConnectionString("DefaultConnection"), true);
            redisConf.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConf);
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
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            return _applicationBuilder;
        }
    }
}
