using MediatR;

namespace Stock_Maintenance_System_Application.User.UpdateCommand;
public record UpdateCommand(
    string FirstName,
    string? LastName,
    string Username,
    string PasswordHash,
    string? Email,
    bool IsActive,
    DateTime PasswordLastChanged,
    bool IsPasswordExpired,
    DateTime? LastLogin,
    string? CreatedBy,
    DateTime CreatedDate,
    string? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<int>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}