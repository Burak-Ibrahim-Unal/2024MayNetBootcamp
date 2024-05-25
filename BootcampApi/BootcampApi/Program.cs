using Bootcamp.Api.Extensions;
using Bootcamp.Repository.Context;
using Bootcamp.Repository.Data;
using Bootcamp.Service.Extensions;
using Bootcamp.Service.Token;
using BootcampApi.Filters;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddService(builder.Configuration);

builder.Services.AddIdentity<AppUser, AppRole>(identityOptions =>
{
    identityOptions.User.RequireUniqueEmail = true;
    identityOptions.Password.RequireDigit = true;
    identityOptions.Password.RequireLowercase = false;
    identityOptions.Password.RequireUppercase = false;
    identityOptions.Password.RequireNonAlphanumeric = false;
    identityOptions.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());

builder.Services.AddAuthentication(authenticationOptions =>
{
    authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, schemeOptions =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>()!;
    schemeOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidateIssuer = true,

        ValidAudiences = tokenOptions.Audience,
        ValidateAudience = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Signature)),
        ValidateIssuerSigningKey = true,

        ValidateLifetime = true
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.SeedDatabase();

app.AddMiddlewares();

app.Run();
