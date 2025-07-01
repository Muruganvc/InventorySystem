using Database_Utility;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using InventorySystem_Api.Common;
using InventorySystem_Api.EndPoints;
using InventorySystem_Application.Company.Command.CreateCommand;
using InventorySystem_Application.Company.Command.UpdateCommand;
using InventorySystem_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
using InventorySystem_Application.Order.Command.OrderCreateCommand;
using InventorySystem_Application.Product.Command.ActivateProductCommand;
using InventorySystem_Application.Product.Command.CreateProductCommand;
using InventorySystem_Application.Product.Command.UpdateProductCommand;
using InventorySystem_Application.User.CreateCommand;
using InventorySystem_Application.User.LoginCommand;
using InventorySystem_Application.User.PasswordChangeCommand;
using InventorySystem_Application.User.UpdateCommand;
using InventorySystem_Domain.Common;
using InventorySystem_Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(UserCreateCommand).Assembly);
builder.Services.AddMediatR(typeof(PasswordChangeCommand).Assembly);
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddMediatR(typeof(LoginCommand).Assembly);
builder.Services.AddMediatR(typeof(UpdateCommand).Assembly);
builder.Services.AddMediatR(typeof(UpdateProductCommand).Assembly);
builder.Services.AddMediatR(typeof(ActivateProductCommand).Assembly);
builder.Services.AddMediatR(typeof(OrderCreateCommand).Assembly);
builder.Services.AddMediatR(typeof(CompanyCreateCommand).Assembly);
builder.Services.AddMediatR(typeof(CompanyUpdateCommand).Assembly);
builder.Services.AddMediatR(typeof(AddOrRemoveUserMenuItemCommand).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateCommandValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(UserCreateCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IDatabaseScriptService, DatabaseScriptService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = ClaimTypes.Role,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    // Add JWT Bearer definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below. Format: Bearer {your token}"
    });

    // Require token for endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
        await context.Response.WriteAsJsonAsync(new { Errors = errors });
    }
});
app.MapUserEndpoints().MapProductCompanyEndpoints().MapProductEndpoints().MapOrderEndpoints();
app.UseHttpsRedirection();

app.Run();
