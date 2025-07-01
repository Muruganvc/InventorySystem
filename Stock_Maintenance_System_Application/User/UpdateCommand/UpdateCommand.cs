using MediatR;

namespace InventorySystem_Application.User.UpdateCommand;
public record UpdateCommand(
    int UserId,
    string FirstName,
    string? LastName, 
    string? Email,
    bool IsActive,
    bool IsSuperAdmin
) : IRequest<bool>;