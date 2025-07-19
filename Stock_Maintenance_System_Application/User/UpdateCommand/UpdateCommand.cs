using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.UpdateCommand;
public record UpdateCommand(
    int UserId,
    string FirstName,
    string? LastName, 
    string? Email, 
    byte[]? ImageData
) : IRequest<IResult<bool>>;