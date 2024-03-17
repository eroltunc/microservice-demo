using System.Reflection;
using CatalogService.Infrastructure;
using CatalogService.Application;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Costum Registrations
builder.Services.AddApplicationRegistration(builder.Configuration);
builder.Services.AddAuthenticationApplicationServiceRegistration(builder.Configuration);
builder.Services.AddAutoMapperApplicationServiceRegistration();
builder.Services.AddMediatRApplicationServiceRegistration();
builder.Services.AddConsulApplicationServiceRegistration(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

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
