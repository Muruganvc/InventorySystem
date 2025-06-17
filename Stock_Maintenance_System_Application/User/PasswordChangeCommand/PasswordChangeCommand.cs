using MediatR;
namespace Stock_Maintenance_System_Application.User.PasswordChangeCommand;
public record PasswordChangeCommand(
    int UserId,
    string Username,
    string CurrentPassword,
    string PasswordHash,
    DateTime PasswordLastChanged,
    int? ModifiedBy,
    DateTime? ModifiedDate
) : IRequest<bool>
{
    public DateTime PasswordExpiresAt => PasswordLastChanged.AddDays(30);
}
