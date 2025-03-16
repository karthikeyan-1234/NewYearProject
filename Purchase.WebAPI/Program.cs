using Microsoft.EntityFrameworkCore;

using Purchases.Infrastructure;

using Purchases.Domain.Contracts;
using Purchases.Infrastructure.Contexts;
using Purchases.Services;
using Domain.Contracts;
using Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB("mongodb://192.168.1.20:27017", "PurchaseDB");
});

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(MongoGenericRepository<>));
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ICachingService, InMemoryCachingService>();

//In memory distributed cache
builder.Services.AddMemoryCache();

//builder.Services.AddHostedService<SyncService>();

builder.Services.AddCors(builder =>
{
    builder.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


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
