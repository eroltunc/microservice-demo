using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
//Add services to the container.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddConsul();
builder.Services.AddAuthenticationServiceRegistration(builder.Configuration);
//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(opt =>
//    {
//        opt.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,//hangi sitenin denetleyip denetlemeyeceğini
//            ValidateAudience = false,//izin verilen sitelerin denetlenip denetlenmeyeceği.
//            ValidateLifetime = true,//Token süresi,
//            ValidateIssuerSigningKey = true,//tokenin bize ait olupğ olmadığı kontrol edilir.
//                                            // ValidAudience = configuration["JWT:ValidAudience"],
//                                            //ValidIssuer = configuration["JWT:ValidIssuer"],//
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
//            ClockSkew = TimeSpan.Zero,//token üstüne ekstra süre ekler
//        };
//    });
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseOcelot().GetAwaiter().GetResult();
app.Run();
