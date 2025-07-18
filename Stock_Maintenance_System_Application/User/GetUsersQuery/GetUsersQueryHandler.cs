using MediatR;
using Microsoft.AspNetCore.Http;
using InventorySystem_Domain.Common;
using System.Security.Claims;
using InventorySystem_Application.Common;

namespace InventorySystem_Application.User.GetUsersQuery;
internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IResult<IReadOnlyList<GetUsersQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetUsersQueryHandler(IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult<IReadOnlyList<GetUsersQueryResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        int userId = int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        var users = await _unitOfWork.Repository<InventorySystem_Domain.User>().GetListByAsync(a => a.UserId != userId);
        var result = users.Select(user => new GetUsersQueryResponse
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            UserName = user.Username,
            IsActive = user.IsActive,
            LastLogin = user.LastLogin ?? default,
            SuperAdmin = false // Consider mapping this from domain if available
        }).OrderBy(a => a.UserName).ToList();

        return Result<IReadOnlyList<GetUsersQueryResponse>>.Success(result);
    }
}
