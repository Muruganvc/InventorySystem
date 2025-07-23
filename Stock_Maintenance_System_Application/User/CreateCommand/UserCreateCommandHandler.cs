using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.User.CreateCommand;
internal sealed class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IUserInfo _userInfo;
    public UserCreateCommandHandler(IUnitOfWork unitOfWork, IUserInfo userInfo)
    {
        _unitOfWork = unitOfWork; 
        _userInfo = userInfo;
    }
    public async Task<IResult<int>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var user = new InventorySystem_Domain.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome2627"),
            Email = request.Email,
            IsActive = request.IsActive,
            PasswordLastChanged = request.PasswordLastChanged,
            IsPasswordExpired = request.IsPasswordExpired,
            LastLogin = request.LastLogin,
            CreatedBy = _userInfo.UserId,
            CreatedDate = DateTime.Now,
            MobileNo = request.MobileNo,
        };

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<InventorySystem_Domain.User>().AddAsync(user);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.Repository<InventorySystem_Domain.UserRole>().AddAsync(new InventorySystem_Domain.UserRole
            {
                RoleId = request.RoleId,
                UserId = user.UserId,
            });
            await _unitOfWork.SaveAsync();
        }, cancellationToken);
        return Result<int>.Success(user.UserId);
    }
}
