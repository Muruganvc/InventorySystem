using Database_Utility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Api.Common;
using Stock_Maintenance_System_Application.Customer.Query.GetAllCustomers;
using Stock_Maintenance_System_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
using Stock_Maintenance_System_Application.MenuItem.Query;
using Stock_Maintenance_System_Application.MenuItem.Query.GetAllMenuItem;
using Stock_Maintenance_System_Application.Product.Command.ActivateProductCommand;
using Stock_Maintenance_System_Application.User.ActiveUserCommand;
using Stock_Maintenance_System_Application.User.CreateCommand;
using Stock_Maintenance_System_Application.User.GetMenuItemPermissionQuery;
using Stock_Maintenance_System_Application.User.GetUserQuery;
using Stock_Maintenance_System_Application.User.GetUsersQuery;
using Stock_Maintenance_System_Application.User.LoginCommand;
using Stock_Maintenance_System_Application.User.PasswordChangeCommand;
using Stock_Maintenance_System_Application.User.UpdateCommand;

namespace Stock_Maintenance_System_Api.EndPoints
{
    public static class UserEndPoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/user-login", async (
                            LoginRequest loginRequest,
                            IMediator mediator) =>
               {
                   var command = new LoginCommand(loginRequest.UserName, loginRequest.Password);
                   var result = await mediator.Send(command);
                   return Results.Ok(new
                   {
                       message = "Login successful",
                       data = result
                   });
               }).WithMetadata(new AllowAnonymousAttribute());

            app.MapPost("/new-user", async (NewUserRequest user, IMediator mediator) =>
            {
                var command = new UserCreateCommand(user.FirstName, user.LastName, user.UserName, user.EmailId, user.IsActive, DateTime.Now,
                    false, user.Role, DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(new { message = "User created successfully", data = result });
            })
            .WithName("UserCreateCommand")
            .WithTags("NewUserCreate")
            .Produces<int>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

            app.MapGet("/users", async (IMediator mediator) =>
            {
                var query = new GetUsersQuery();
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "ALl Users", data = result });
            }) 
           .RequireAuthorization("AdminOnly");

            app.MapPut("/password-change/{UserId}", async (int UserId, ChangePasswordRequest user, IMediator mediator) =>
            {
                var command = new PasswordChangeCommand(UserId, user.UserName,user.CurrentPassword, user.PasswordHash, DateTime.Now, 1,
                    DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(new { message = "Password Changed successfully", data = result });
            }).RequireAuthorization();

            app.MapGet("/user/{userName}", async (string userName, IMediator mediator) =>
            {
                var query = new GetUserQuery(userName);
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "Current User", data = result });
            }).RequireAuthorization();

            app.MapGet("/menu/{UserId}", async (int UserId, IMediator mediator) =>
            {
                var query = new GetMenuItemPermissionQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "User Menu", data = result });
            }).RequireAuthorization();

            app.MapPost("/menu/{UserId}/{MenuId}", async (int UserId, int MenuId, IMediator mediator) =>
            {
                var query = new AddOrRemoveUserMenuItemCommand(UserId, MenuId);
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "Add or Remove User menu List", data = result });
            }).RequireAuthorization();

            app.MapPut("/update/{UserId}", async (int UserId, UpdateUserRequest user, IMediator mediator) =>
            {
                var command = new UpdateCommand(UserId, user.FirstName, user.LastName, user.Email, user.IsActive, user.IsSuperAdmin);
               var result = await mediator.Send(command);
                return Results.Ok(new { message = "User Updated successfully", data = result });
            }).RequireAuthorization();

            app.MapPut("/user/activate/{userId}", async (int userId,
          IMediator mediator) =>
            {
                var command = new ActiveUserCommand(userId);
                var result = await mediator.Send(command);
                return Results.Ok(new
                {
                    message = "Product Update successfully",
                    data = result
                });
            }).RequireAuthorization("AdminOnly");


            app.MapGet("/menus/{UserId}", async (int UserId,IMediator mediator) =>
            {
                var query = new GetMenuItemQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "User Menu Lists", data = result });
            }).RequireAuthorization();

            app.MapGet("/menus", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllMenuItemQuery());
                return Results.Ok(new { message = "All Menu Lists", data = result });
            }).RequireAuthorization();

            app.MapGet("/customers", async(IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllCustomersQuery());
                return Results.Ok(new { message = "All customer Lists", data = result });
            }).RequireAuthorization();

            app.MapPost("/database-backup", async (string userName, IDatabaseScriptService DatabaseScriptService) =>
            {
                var result = DatabaseScriptService.GenerateFullDatabaseScript(userName);
                return Results.Ok(new { message = "Database backup details", data = result });
            }).RequireAuthorization();

            app.MapGet("/backup", async (IConfiguration configuration, IDatabaseScriptService DatabaseScriptService) =>
            {
                string? fileName = $"{configuration["appSetting:backUpHistory"]}";
                var result = DatabaseScriptService.ReadCsv(fileName);
                return Results.Ok(new { message = "All customer Lists", data = result });
            }).RequireAuthorization();

            return app;
        }
    }
}