using MediatR;

namespace Stock_Maintenance_System_Application.User.LoginCommand;
public record LoginCommand(string UserName, string Password)
    :IRequest<LoginCommandResponse>;
