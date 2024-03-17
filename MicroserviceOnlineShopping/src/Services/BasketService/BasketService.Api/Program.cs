using BasketService.Application.IntegrationEvents.EventHandlers;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Business;
using EventBus.Base.Abstraction;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Costum Registrations
builder.Services.AddBusinessRegistration(builder.Configuration);
builder.Services.AddIntegrationEventServiceRegistration();
builder.Services.AddAuthenticationServiceRegistration(builder.Configuration);
builder.Services.AddConsulServiceRegistration(builder.Configuration);
builder.Services.AddRedisServiceRegistration(builder.Configuration);

builder.Services.AddHttpContextAccessor();
var app = builder.Build();
//Costum Registrations
app.AddIntegrationEventServiceRegistrationBuilder();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//Costum Registrations
app.AddConsulServiceRegistrationBuilder(app.Lifetime,app.Configuration);
app.Run();
