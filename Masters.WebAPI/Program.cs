using Masters.Domain.Contracts;
using Masters.Infrastructure;
using Masters.Infrastructure.Contexts;
using Sales.Services;

using Microsoft.EntityFrameworkCore;
using Masters.Services;
using Domain.Contracts;
using Infrastructure;
using Serilog;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogs();

// Add services to the container.

builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB("mongodb://192.168.1.20:27017", "ProductsDB");
});

builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(MongoGenericRepository<>));
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // Set the metadata address for the OpenID configuration
        options.MetadataAddress = builder.Configuration["Keycloak:OpenIdConfigMetaAddr"]!;
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:ClientId"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],
            ValidAudience = builder.Configuration["Keycloak:Audience"],
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                var claims = context.Principal!.Claims.ToList();

                // Extract roles from "resource_access.angular-app.roles"
                var resourceAccess = claims.FirstOrDefault(c => c.Type == "resource_access");
                if (resourceAccess != null)
                {
                    //Deserialize the resource access claim
                    var parsedResourceAccess = JObject.Parse(resourceAccess.Value);
                    var angularAppRoles = parsedResourceAccess["angular-app"]?["roles"]?.ToObject<List<string>>();

                    if (angularAppRoles != null)
                    {
                        foreach (var role in angularAppRoles)
                        {
                            claimsIdentity!.AddClaim(new Claim(ClaimTypes.Role, role)); // ✅ Map roles properly
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();



void ConfigureLogs()
{
    var environment = builder.Environment;
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://192.168.1.20:9200/"))
        {
            AutoRegisterTemplate = true,
            IndexFormat = "logstash-{0:yyyy.MM.dd}",
            InlineFields = true
        })
        .CreateLogger();
}