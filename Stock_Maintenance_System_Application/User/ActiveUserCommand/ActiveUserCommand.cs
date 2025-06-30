using MediatR;

namespace Stock_Maintenance_System_Application.User.ActiveUserCommand;
public record ActiveUserCommand(int UserId) : IRequest<bool>;