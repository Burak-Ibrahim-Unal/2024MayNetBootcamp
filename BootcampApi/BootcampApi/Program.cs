using Bootcamp.Api.Extensions;
using Bootcamp.Repository.Context;
using Bootcamp.Repository.Data;
using Bootcamp.Service.Extensions;
using Bootcamp.Service.Token;
using Bootcamp.Service.Users;
using BootcampApi.Filters;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddService(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, OverAgeRequirementHandler>();

builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("Over18AgePolicy", x => { x.AddRequirements(new OverAgeRequirement() { Age = 10 }); });


    x.AddPolicy("UpdatePolicy", y => { y.RequireClaim("update", "true"); });
});

var app = builder.Build();

app.SeedDatabase();
await app.SeedIdentityData();

app.AddMiddlewares();

app.Run();
