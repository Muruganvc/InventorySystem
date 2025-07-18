using Database_Utility;
using InventorySystem_Api.ApiRequest;
using InventorySystem_Application.Customer.Query.GetAllCustomers;
using InventorySystem_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
using InventorySystem_Application.MenuItem.Query;
using InventorySystem_Application.MenuItem.Query.GetAllMenuItem;
using InventorySystem_Application.User.ActiveUserCommand;
using InventorySystem_Application.User.CreateCommand;
using InventorySystem_Application.User.GetMenuItemPermissionQuery;
using InventorySystem_Application.User.GetUserQuery;
using InventorySystem_Application.User.GetUsersQuery;
using InventorySystem_Application.User.LoginCommand;
using InventorySystem_Application.User.PasswordChangeCommand;
using InventorySystem_Application.User.UpdateCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InventorySystem_Api.EndPoints
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
                   return Results.Ok(result);
               }).WithMetadata(new AllowAnonymousAttribute());

            app.MapPost("/new-user", async (NewUserRequest user, IMediator mediator) =>
            {
                var command = new UserCreateCommand(user.FirstName, user.LastName, user.UserName, user.EmailId, user.IsActive, DateTime.Now,
                    false, user.Role, DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(result);
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
                return Results.Ok(result);
            })
           .RequireAuthorization("AdminOnly");

            app.MapPut("/password-change/{UserId}", async (int UserId, ChangePasswordRequest user, IMediator mediator) =>
            {
                var command = new PasswordChangeCommand(UserId, user.UserName, user.CurrentPassword, user.PasswordHash, DateTime.Now, 1,
                    DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet("/user/{userName}", async (string userName, IMediator mediator) =>
            {
                var query = new GetUserQuery(userName);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet("/menu/{UserId}", async (int UserId, IMediator mediator) =>
            {
                var query = new GetMenuItemPermissionQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapPost("/menu/{UserId}/{MenuId}", async (int UserId, int MenuId, IMediator mediator) =>
            {
                var query = new AddOrRemoveUserMenuItemCommand(UserId, MenuId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapPut("/update/{UserId}", async (int UserId, UpdateUserRequest user, IMediator mediator) =>
            {
                var command = new UpdateCommand(UserId, user.FirstName, user.LastName, user.Email, user.IsActive, user.IsSuperAdmin);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapPut("/user/activate/{userId}", async (int userId,
          IMediator mediator) =>
            {
                var command = new ActiveUserCommand(userId);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("AdminOnly");


            app.MapGet("/menus/{UserId}", async (int UserId, IMediator mediator) =>
            {
                var query = new GetMenuItemQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet("/menus", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllMenuItemQuery());
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet("/customers", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllCustomersQuery());
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapPost("/database-backup", (IConfiguration config, string userName, IDatabaseScriptService DatabaseScriptService) =>
            {
                var backUpHistory = config["appSetting:backUpHistory"];
                var result = DatabaseScriptService.GenerateFullDatabaseScript(userName, backUpHistory);
                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet("/backup", (IConfiguration config, IDatabaseScriptService databaseScriptService) =>
            {
                // Read settings
                var backupPath = config["appSetting:backUpPath"];
                var fileName = config["appSetting:backUpHistory"];

                if (string.IsNullOrWhiteSpace(backupPath) || string.IsNullOrWhiteSpace(fileName))
                {
                    return Results.BadRequest(new { message = "Backup path or filename is not configured properly." });
                }
                var fullFileName = Path.Combine(backupPath, fileName);
                // Ensure file exists before proceeding
                if (!File.Exists(fullFileName))
                {
                    return Results.NotFound(new { message = "Backup file not found." });
                }
                var data = databaseScriptService.ReadCsv(fullFileName);
                return Results.Ok(new { data });
            }).RequireAuthorization();

            return app;
        }
    }
}