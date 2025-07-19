using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventorySystem_Application.User.CreateCommand;
internal sealed class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, IResult<int>>
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserCreateCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork; 
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IResult<int>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
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
            CreatedBy = userId,
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
