using MediatR;
using Stock_Maintenance_System_Domain.Common;

namespace Stock_Maintenance_System_Application.User.GetUsersQuery;
internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<GetUsersQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetUsersQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public async Task<IReadOnlyList<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Repository<Stock_Maintenance_System_Domain.User.User>().GetAllAsync();
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
