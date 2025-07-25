using Database_Utility;
using FluentValidation;
using InventorySystem_Api.Common;
using InventorySystem_Api.EndPoints;
using InventorySystem_Application.Common;
using InventorySystem_Application.Company.Command.CreateCommand;
using InventorySystem_Application.Company.Command.UpdateCommand;
using InventorySystem_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
using InventorySystem_Application.Order.Command.OrderCreateCommand;
using InventorySystem_Application.Product.Command.ActivateProductCommand;
using InventorySystem_Application.Product.Command.CreateProductCommand;
using InventorySystem_Application.Product.Command.QuantityUpdateCommand;
using InventorySystem_Application.Product.Command.UpdateProductCommand;
using InventorySystem_Application.User.CreateCommand;
using InventorySystem_Application.User.LoginCommand;
using InventorySystem_Application.User.PasswordChangeCommand;
using InventorySystem_Application.User.UpdateCommand;
using InventorySystem_Domain.Common;
using InventorySystem_Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
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
builder.Services.AddMediatR(typeof(QuantityUpdateCommand).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateCommandValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(UserCreateCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IDatabaseScriptService, DatabaseScriptService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserInfo, UserInfo>();
var config = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

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
    options.AddPolicy("AllRoles", policy =>
        policy.RequireRole("Admin", "SuperAdmin", "User"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireRole("User"));

    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", policy =>
    {
        policy.WithOrigins(config["appSetting:cors"]!)
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

var logDirectory = config["appSetting:errorLogPath"] ?? @"C:\Logs\InventorySystem";
var logFilePath = Path.Combine(logDirectory, "errors.txt");

// Ensure log directory exists
Directory.CreateDirectory(logDirectory); // Safe even if it already exists

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File(
        path: logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 365, // Retain logs for one year
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowCors");

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
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
app.MapUserEndpoints().MapProductCompanyEndpoints().MapProductEndpoints().MapOrderEndpoints().MapDashboardEndpoints();
app.UseHttpsRedirection();

app.Run();
