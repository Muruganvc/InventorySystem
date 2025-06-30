using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stock_Maintenance_System_Domain.Common;
using System.Security.Claims;

namespace Stock_Maintenance_System_Application.User.CreateCommand;
internal sealed class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserCreateCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<int> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        //string pwd = _configuration["_configuration:defaultPwd"]!;
        //if (string.IsNullOrEmpty(pwd)) { return 0; }
        var user = new Stock_Maintenance_System_Domain.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash = "Welcome2627",
            Email = request.Email,
            IsActive = request.IsActive,
            PasswordLastChanged = request.PasswordLastChanged,
            IsPasswordExpired = request.IsPasswordExpired,
            LastLogin = request.LastLogin,
            CreatedBy = userId,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.User>().AddAsync(user);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.Repository<Stock_Maintenance_System_Domain.UserRole>().AddAsync(new Stock_Maintenance_System_Domain.UserRole
            {
                RoleId = request.RoleId,
                UserId = user.UserId,
            });
            await _unitOfWork.SaveAsync();
        }, cancellationToken);
        return user.UserId;
    }
}
