using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;

namespace InventorySystem_Application.User.GetUsersQuery;
internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<GetUsersQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetUsersQueryHandler(IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork; 
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IReadOnlyList<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var users = await _unitOfWork.Repository<InventorySystem_Domain.User>().GetListByAsync(a => a.UserId != userId);
        return users.Select(user => new GetUsersQueryResponse
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            UserName = user.Username,
            IsActive = user.IsActive,
            LastLogin = user.LastLogin ?? default,
            SuperAdmin = false // Consider mapping this from domain if available
        }).ToList();
    }
}
