using MediatR;
namespace Stock_Maintenance_System_Application.User.PasswordChangeCommand;
public record PasswordChangeCommand(
    string Username,
    string PasswordHash,
    DateTime PasswordLastChanged,
    string? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<bool>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}
