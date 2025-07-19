using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.User.CreateCommand;
public record UserCreateCommand(
    string FirstName,
    string? LastName,
    string Username,
    string? Email,
    bool IsActive,
    DateTime PasswordLastChanged,
    bool IsPasswordExpired,
    int RoleId,
    DateTime? LastLogin,
    string MobileNo
) : IRequest<IResult<int>>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}
