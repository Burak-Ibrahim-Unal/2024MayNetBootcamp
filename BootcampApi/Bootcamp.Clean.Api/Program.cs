using Bootcamp.Clean.Api.Extensions;
using Bootcamp.Clean.ApplicationService;
using Bootcamp.Clean.ApplicationService.Interfaces;
using Bootcamp.Clean.ApplicationService.ProductService;
using Bootcamp.Clean.ApplicationService.ProductService.Configurations;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.ProductService.Service;
using Bootcamp.Clean.Cache.InMemoryCache;
using Bootcamp.Clean.Cache.RedisCache;
using Bootcamp.Clean.Repository;
using Bootcamp.Clean.Repository.Context;
using Bootcamp.Clean.Repository.Repositories;
using Bootcamp.Clean.Repository.Repositories.ProductRepositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddScoped<NotFoundFilter>();
builder.Services.AddSingleton<PriceCalculator>();

builder.Services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();

builder.Services.AddSingleton<ICustomCacheService, RedisCacheService>(options =>
{
    return new RedisCacheService(builder.Configuration.GetConnectionString("Redis")!);
});
builder.Services.AddScoped<ICacheService, InMemoryCacheService>();
builder.Services.AddMemoryCache();

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
