using MediatR;
using Stock_Maintenance_System_Application.User.CreateCommand;
using Stock_Maintenance_System_Application.User.PasswordChangeCommand;
using Stock_Maintenance_System_Domain.User;

namespace Stock_Maintenance_System_Api.EndPoints
{
    public static class UserEndPoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/new-user", async (User user, IMediator mediator) =>
            {
                var command = new UserCreateCommand(user.FirstName, user.LastName, user.Username, user.PasswordHash, user.Email,
                    user.IsActive, user.PasswordLastChanged, user.IsPasswordExpired, user.LastLogin, user.CreatedBy, user.CreatedDate, user.ModifiedBy,
                    user.ModifiedDate);
                await mediator.Send(command);
                return Results.Ok(new { message = "User created successfully", user });
            });

            app.MapPut("/password-change", async (User user, IMediator mediator) =>
            {
                var command = new PasswordChangeCommand(user.Username, user.PasswordHash, user.PasswordLastChanged, user.ModifiedBy,
                    user.ModifiedDate);
                await mediator.Send(command);
                return Results.Ok(new { message = "User created successfully", user });
            });

            return app;
        }
    }
}
