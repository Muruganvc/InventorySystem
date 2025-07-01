namespace InventorySystem_Application.User.LoginCommand;
public record LoginCommandResponse(int UserId, string FirstName, string LastName, string Email, string UserName, string Token);
