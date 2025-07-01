using MediatR;

namespace InventorySystem_Application.User.LoginCommand;
public record LoginCommand(string UserName, string Password)
    :IRequest<LoginCommandResponse>;
