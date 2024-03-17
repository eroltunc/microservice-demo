using EventBus.Base.Abstraction;

using IdentityService.Business.IntegrationEvents.Events;
using IdentityService.Business;
using IdentityService.DataAccess.Contexts;
using IdentityService.Entity.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Costum Registrations
builder.Services.AddBusinessRegistration(builder.Configuration);
builder.Services.AddIntegrationEventServiceRegistration();
builder.Services.AddAuthenticationServiceRegistration(builder.Configuration);
builder.Services.AddConsulServiceRegistration(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Costum Registrations
app.AddConsulServiceRegistrationBuilder(app.Lifetime, app.Configuration);
app.Run();
