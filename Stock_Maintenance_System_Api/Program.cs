using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock_Maintenance_System_Api.EndPoints;
using Stock_Maintenance_System_Application.Product.CreateCommand;
using Stock_Maintenance_System_Application.User.CreateCommand;
using Stock_Maintenance_System_Application.User.PasswordChangeCommand;
using Stock_Maintenance_System_Domain.Common;
using Stock_Maintenance_System_Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(UserCreateCommand).Assembly);
builder.Services.AddMediatR(typeof(PasswordChangeCommand).Assembly);
builder.Services.AddMediatR(typeof(ProductCreateCommand).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateCommandValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(UserCreateCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();
 
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
app.MapUserEndpoints();
app.UseHttpsRedirection();

app.Run();
 