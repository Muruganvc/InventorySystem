using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.CreateCommand;
internal sealed class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    public UserCreateCommandHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<int> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var user = new Stock_Maintenance_System_Domain.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash = request.PasswordHash,
            Email = request.Email,
            IsActive = request.IsActive,
            PasswordLastChanged = request.PasswordLastChanged,
            IsPasswordExpired = request.IsPasswordExpired,
            LastLogin = request.LastLogin,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            ModifiedBy = request.ModifiedBy,
            ModifiedDate = request.ModifiedDate
        };

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.User>().AddAsync(user);
            await _unitOfWork.SaveAsync();
        });

        return user.UserId;
    }
}
