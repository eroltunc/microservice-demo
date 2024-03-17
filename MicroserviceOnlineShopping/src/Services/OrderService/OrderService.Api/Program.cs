using EventBus.Base.Abstraction;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Costum Registrations
builder.Services.AddApplicationRegistration(builder.Configuration);
builder.Services.AddAuthenticationApplicationServiceRegistration(builder.Configuration);
builder.Services.AddAutoMapperApplicationServiceRegistration();
builder.Services.AddMediatRApplicationServiceRegistration();
builder.Services.AddConsulApplicationServiceRegistration(builder.Configuration);
builder.Services.AddIntegrationEventServiceRegistration();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();
var app = builder.Build();
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
app.AddIntegrationEventServiceRegistrationBuilder();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.AddConsulApplicationServiceRegistrationBuilder(app.Lifetime, app.Configuration);
app.Run();
