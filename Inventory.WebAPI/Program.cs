using Domain.Contracts;

using Infrastructure;

using Inventory.Domain.Contracts;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Contexts;
using Inventory.Services;
using Inventory.Services.Contracts;

using Microsoft.EntityFrameworkCore;

using Serilog;

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
    options.UseMongoDB("mongodb://192.168.1.20:27017", "InventoryDB");
});

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(MongoGenericRepository<>));
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddHostedService<InventoryBackGroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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
