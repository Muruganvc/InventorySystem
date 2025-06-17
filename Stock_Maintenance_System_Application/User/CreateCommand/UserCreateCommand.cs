using MediatR;

namespace Stock_Maintenance_System_Application.User.CreateCommand;
public record UserCreateCommand(
    string FirstName,
    string? LastName,
    string Username,
    string PasswordHash,
    string? Email,
    bool IsActive,
    DateTime PasswordLastChanged,
    bool IsPasswordExpired,
    DateTime? LastLogin,
    int CreatedBy,
    DateTime CreatedDate,
    int? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<int>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}
