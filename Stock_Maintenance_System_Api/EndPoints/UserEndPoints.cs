using MediatR;
using Microsoft.AspNetCore.Authorization;
using Stock_Maintenance_System_Api.ApiRequest;
using Stock_Maintenance_System_Application.Customer.Query.GetAllCustomers;
using Stock_Maintenance_System_Application.MenuItem.Query;
using Stock_Maintenance_System_Application.MenuItem.Query.GetAllMenuItem;
using Stock_Maintenance_System_Application.User.CreateCommand;
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
                var command = new UserCreateCommand(user.FirstName, user.LastName, user.UserName, user.Password, user.EmailId,
                    user.IsActive, DateTime.Now, false, DateTime.Now, 1, DateTime.Now, null, null);
                await mediator.Send(command);
                return Results.Ok(new { message = "User created successfully", data = user });
            });

            app.MapGet("/users", async (IMediator mediator) =>
            {
                var query = new GetUsersQuery();
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "", data = result });
            });

            app.MapPut("/password-change/{UserId}", async (int UserId, ChangePasswordRequest user, IMediator mediator) =>
            {
                var command = new PasswordChangeCommand(UserId, user.UserName,user.CurrentPassword, user.PasswordHash, DateTime.Now, 1,
                    DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(new { message = "Password Changed successfully", data = result });
            });

            app.MapPut("/update/{UserId}", async (int UserId, UpdateUserRequest user, IMediator mediator) =>
            {
                var command = new UpdateCommand(UserId, user.FirstName, user.LastName, user.EmailId, user.IsActive, user.IsSuperAdmin);
               var result = await mediator.Send(command);
                return Results.Ok(new { message = "User Updated successfully", data = result });
            });


            app.MapGet("/menus/{UserId}", async (int UserId,IMediator mediator) =>
            {
                var query = new GetMenuItemQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(new { message = "User Menu Lists", data = result });
            });

            app.MapGet("/menus", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllMenuItemQuery());
                return Results.Ok(new { message = "All Menu Lists", data = result });
            });

            app.MapGet("/customers", async(IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllCustomersQuery());
                return Results.Ok(new { message = "All customer Lists", data = result });
            });

            return app;
        }
    }
}