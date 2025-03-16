using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

using Sales.Domain.Contracts;
using Sales.Infrastructure;
using Sales.Infrastructure.Contexts;
using Sales.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB("mongodb://192.168.1.20:27017", "SalesDB");
});

builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(MongoGenericRepository<>));
builder.Services.AddScoped<IMessageService, MessageService>();
//builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<ICachingService, InMemoryCachingService>();

builder.Services.AddCors(builder =>
{
    builder.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//In memory distributed cache
builder.Services.AddMemoryCache();

//builder.Services.AddHostedService<SyncService>();


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

app.UseCors("AllowAll");

app.Run();
