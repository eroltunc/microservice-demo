using Consul;
using EventBus.Base;
using EventBus.Factory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Mapping;
using System.Reflection;
using System.Text;


namespace OrderService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIntegrationEventServiceRegistration(this IServiceCollection _services)
        {
            _services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "OrderService",
                    EventBusType = EventBusType.RabbitMQ,
                };
                return EventBusFactory.Create(config, sp);
            });
            
            return _services;
        }
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {

            return _services;
        }
        public static IServiceCollection AddConsulApplicationServiceRegistration(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(_configuration["ConsulConfiguration:ConsulAddress"]);
            }));
            return _services;
        }
        public static IServiceCollection AddMediatRApplicationServiceRegistration(this IServiceCollection _services)
        {
            _services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return _services;
        }
        public static IServiceCollection AddAutoMapperApplicationServiceRegistration(this IServiceCollection _services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            _services.AddSingleton(mapper);
            return _services;
        }
        public static IServiceCollection AddAuthenticationApplicationServiceRegistration(this IServiceCollection _services, IConfiguration _configuration)
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
        public static IApplicationBuilder AddConsulApplicationServiceRegistrationBuilder(this IApplicationBuilder _applicationBuilder, IHostApplicationLifetime _hostApplicationLifetime, IConfiguration _configuration)
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
            //IEventBus eventBus = _applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            //eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            return _applicationBuilder;
        }
    }
}
