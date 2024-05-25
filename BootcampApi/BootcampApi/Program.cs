using Bootcamp.Api.Extensions;
using Bootcamp.Repository.Data;
using Bootcamp.Service.Extensions;
using BootcampApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddService(builder.Configuration);

builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.SeedDatabase();

app.AddMiddlewares();

app.Run();
