using Bootcamp.Repository;
using Bootcamp.Repository.Context;
using Bootcamp.Repository.Data;
using Bootcamp.Repository.Repositories;
using Bootcamp.Service;
using Bootcamp.Service.ProductService.Configurations;
using BootcampApi.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        x => { x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name); });
});

builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });

builder.Services.AddAutoMapper(typeof(ServiceAssembly).Assembly);

builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddProductService();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.SeedDatabase();

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
