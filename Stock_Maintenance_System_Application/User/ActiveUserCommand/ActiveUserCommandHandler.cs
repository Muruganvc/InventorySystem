using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;

namespace InventorySystem_Application.User.ActiveUserCommand;

internal sealed class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    public ActiveUserCommandHandler(IUnitOfWork unitOfWork,
        IRepository<InventorySystem_Domain.User> userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public  async Task<bool> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var user = await _userRepository.GetByAsync(a => a.UserId == request.UserId);
        if (user == null) return false;
        user.IsActive = !user.IsActive;
        user.ModifiedBy = userId;
        user.ModifiedDate = DateTime.Now;
        bool isSuccess = false;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            isSuccess = await _unitOfWork.SaveAsync() > 0;
        }, cancellationToken);
        return isSuccess;
    }
}
