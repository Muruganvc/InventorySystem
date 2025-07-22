using Database_Utility;
using InventorySystem_Api.ApiRequest;
using InventorySystem_Application.Customer.Query.GetAllCustomers;
using InventorySystem_Application.InventoryCompanyInfo.CreateInventoryCompanyInfoCommand;
using InventorySystem_Application.InventoryCompanyInfo.GetInventoryCompanyInfoQuery;
using InventorySystem_Application.InventoryCompanyInfo.UpdateInventoryCompanyInfoCommand;
using InventorySystem_Application.MenuItem.AddOrRemoveUserMenuItemCommand;
using InventorySystem_Application.MenuItem.Query;
using InventorySystem_Application.MenuItem.Query.GetAllMenuItem;
using InventorySystem_Application.User.ActiveUserCommand;
using InventorySystem_Application.User.CreateCommand;
using InventorySystem_Application.User.ForgetPasswordCommand;
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
                    false, user.Role, DateTime.Now,user.MobileNo);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("UserCreateCommand")
            .WithTags("NewUserCreate")
            .Produces<int>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("SuperAdminOnly");

            app.MapGet("/users", async (IMediator mediator) =>
            {
                var query = new GetUsersQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
           .RequireAuthorization("AllRoles");

            app.MapPut("/password-change/{UserId}", async (int UserId, ChangePasswordRequest user, IMediator mediator) =>
            {
                var command = new PasswordChangeCommand(UserId, user.UserName, user.CurrentPassword, user.PasswordHash, DateTime.Now, 1,
                    DateTime.Now);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapGet("/user/{userName}", async (string userName, IMediator mediator) =>
            {
                var query = new GetUserQuery(userName);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapGet("/menu/{UserId}", async (int UserId, IMediator mediator) =>
            {
                var query = new GetMenuItemPermissionQuery(UserId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapPost("/menu/{UserId}/{MenuId}", async (int UserId, int MenuId, IMediator mediator) =>
            {
                var query = new AddOrRemoveUserMenuItemCommand(UserId, MenuId);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            //app.MapPut("/update/{UserId}", async (int UserId, UpdateUserRequest user, IMediator mediator) =>
            //{
            //    var command = new UpdateCommand(UserId, user.FirstName, user.LastName, user.Email, user.IsActive, user.IsSuperAdmin,null);
            //    var result = await mediator.Send(command);
            //    return Results.Ok(result);
            //}).RequireAuthorization();


            app.MapPut("/update/{UserId}", async (HttpRequest request, int UserId, IMediator mediator) =>
            {
                var form = await request.ReadFormAsync();

                var firstName = form["FirstName"];
                var lastName = form["LastName"];
                var email = form["Email"];
                var mobileNo = form["mobileNo"];
                IFormFile? imageFile = form.Files["Image"];

                byte[]? imageData = null;
                if (imageFile != null && imageFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await imageFile.CopyToAsync(ms);
                    imageData = ms.ToArray();
                }

                var command = new UpdateCommand(UserId, firstName!, lastName!, email!, imageData, mobileNo);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");



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
            }).RequireAuthorization("AllRoles");

            app.MapGet("/menus", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllMenuItemQuery());
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapGet("/customers", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllCustomersQuery());
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapPost("/database-backup", (IConfiguration config, string userName, IDatabaseScriptService DatabaseScriptService) =>
            {
                var backUpHistory = config["appSetting:backUpHistory"];
                var result = DatabaseScriptService.GenerateFullDatabaseScript(userName, backUpHistory);
                return Results.Ok(new
                {
                    Value = result,
                    IsSuccess = true,
                    Error = string.Empty
                });
            }).RequireAuthorization("AllRoles");

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
                if (!File.Exists(fullFileName))
                {
                    return Results.NotFound(new { message = "Backup file not found." });
                }
                var data = databaseScriptService.ReadCsv(fullFileName);

                var result = new
                {
                    Value = data,
                    IsSuccess = true,
                    Error = string.Empty
                };
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapPut("/forget-password", async (
                        string userName,
                        string mobileNo,
                        IConfiguration config,
                        IMediator mediator) =>
            {
                var defaultPassword = config["appSetting:defaultPwd"]
                    ?? $"Wel{DateTime.Now:yyyy-MM-dd}Come2627";

                var command = new ForgetPasswordCommand(userName, mobileNo, defaultPassword);
                var result = await mediator.Send(command);

                return Results.Ok(result);
            }).WithMetadata(new AllowAnonymousAttribute());

            app.MapPost("/inventory-company-info", async (HttpRequest request, IMediator mediator) =>
            {
                var form = await request.ReadFormAsync();

                var inventoryCompanyInfoName = form["inventoryCompanyInfoName"];
                string? description = form["description"];
                var address = form["address"];
                var email = form["email"];
                var mobileNo = form["mobileNo"];
                var gstNumber = form["gstNumber"];
                var apiVersion = form["apiVersion"];
                var uiVersion = form["uiVersion"];
                var bankName = form["bankName"];
                var bankBranchName = form["bankBranchName"];
                var bankAccountNo = form["bankAccountNo"];
                var bankBranchIFSC = form["bankBranchIFSC"];

                IFormFile? qcCodeFile = form.Files["QcCode"];
                byte[]? qcCodeData = null;

                if (qcCodeFile is { Length: > 0 })
                {
                    using var ms = new MemoryStream();
                    await qcCodeFile.CopyToAsync(ms);
                    qcCodeData = ms.ToArray();
                }

                var command = new CreateInventoryCompanyInfoCommand(
                    inventoryCompanyInfoName!,description!, address!, mobileNo!, gstNumber!, apiVersion!, uiVersion!, qcCodeData!,
                    email!,bankName!,bankBranchName!,bankAccountNo!,bankBranchIFSC!
                );
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).RequireAuthorization("SuperAdminOnly");

            app.MapGet("/inventory-company-info", async (IMediator mediator) =>
            {
                var query = new GetInventoryCompanyInfoQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }).RequireAuthorization("AllRoles");

            app.MapPost("/inventory-company-info/{invCompanyInfoId}", async (
                HttpRequest request,
                int invCompanyInfoId,
                IMediator mediator) =>
            {
                var form = await request.ReadFormAsync();

                string? inventoryCompanyInfoName = form["inventoryCompanyInfoName"];
                string? description = form["description"];
                string? address = form["address"];
                string? email = form["email"];
                string? mobileNo = form["mobileNo"];
                string? gstNumber = form["gstNumber"];
                string? apiVersion = form["apiVersion"];
                string? uiVersion = form["uiVersion"];
                string? bankName = form["bankName"];
                string? bankBranchName = form["bankBranchName"];
                string? bankAccountNo = form["bankAccountNo"];
                string? bankBranchIFSC = form["bankBranchIFSC"];

                // Handle file upload (QRCode)
                byte[]? qcCodeData = null;
                var qcCodeFile = form.Files["QcCode"];
                if (qcCodeFile is { Length: > 0 })
                {
                    using var ms = new MemoryStream();
                    await qcCodeFile.CopyToAsync(ms);
                    qcCodeData = ms.ToArray();
                }

                // Construct command
                var command = new UpdateInventoryCompanyInfoCommand(invCompanyInfoId, inventoryCompanyInfoName!, description!, address!,
                    mobileNo!, gstNumber!, apiVersion!, uiVersion!, qcCodeData!, email!, bankName!, 
                    bankBranchName!, bankAccountNo!, bankBranchIFSC!);

                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .RequireAuthorization("SuperAdminOnly");
            return app;
        }
    }
}