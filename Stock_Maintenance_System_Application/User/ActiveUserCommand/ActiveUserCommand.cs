using MediatR;

namespace InventorySystem_Application.User.ActiveUserCommand;
public record ActiveUserCommand(int UserId) : IRequest<bool>;